using DG.Tweening;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private List<RectTransform> _characterDialogueImages;
    [SerializeField] private GameObject _choicesGroup;
    [SerializeField] private GameObject _choiceBoxPrefab;
    [SerializeField] private List<GameObject> _choiceBoxes = new List<GameObject>();

    [SerializeField] private float _slideEffectDuration;
    [SerializeField] private Vector2 offsetSlidePosition = new Vector2(0f, 0f);
    [SerializeField] private Vector2 leftSlidePosition = new Vector2(1920f, 0f);
    [SerializeField] private Vector2 rightSlidePosition = new Vector2(-1920f, 0f);

    private const string DialogueImage_Label = "[DialogueImage] ";

    private static string playerAnswer = string.Empty;
    private static string npcAnswer = string.Empty;

    public static DialogueManager Instance { get; private set; }

    public Image Forescreen;

    public async UniTask FadeOutEffect()
    {
        await TransitionAlpha(Forescreen, 0f);
        Forescreen.gameObject.SetActive(false);
        Player.player.SetFlip(true);
    }

    public async UniTask FadeInEffect()
    {
        Player.player.SetFlip(false);
        Forescreen.gameObject.SetActive(true);
        await TransitionAlpha(Forescreen, 1f);
    }

    public async UniTask TransitionAlpha(Image targetImage, float targetAlpha)
    {
        float progress = 0f;
        float startAlpha = targetImage.color.a;

        while (progress < 1f)
        {
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, progress);
            Color newColor = targetImage.color;
            newColor.a = currentAlpha;
            targetImage.color = newColor;

            progress += Time.deltaTime / 2f;
            await UniTask.Yield();
        }

        Color finalColor = targetImage.color;
        finalColor.a = targetAlpha;
        targetImage.color = finalColor;
    }


    private async void Awake()
    {
        Instance = this;
        await FadeOutEffect();
    }

    public async UniTask DisplayDialogue(DialogueSO _currentDialogueBox)
    {
        while (_currentDialogueBox != null) 
        {
            foreach (var dialogueLine in _currentDialogueBox.dialogues)
            {
                if (FindCharacter(dialogueLine.Character) != null)
                {
                    RectTransform currentRect = FindCharacter(dialogueLine.Character);
                    Tween tweenIn = AnimateIn(currentRect, SlideEffectSide(currentRect));
                    await UniTask.WaitUntil(() => !tweenIn.IsPlaying());

                    TextMeshProUGUI text = currentRect.GetComponentInChildren<TextMeshProUGUI>();
                    if (dialogueLine.Character == Character.MainCharacter)
                    {
                        if (dialogueLine.Text == string.Empty)
                        {
                            await UniTask.WaitUntil(() => playerAnswer != null);
                            dialogueLine.Text = playerAnswer;
                        }
                    }
                    else
                    {
                        if (dialogueLine.Text == string.Empty)
                        {
                            await UniTask.WaitUntil(() => npcAnswer != null);
                            dialogueLine.Text = npcAnswer;
                        }
                    }
                    text.text = dialogueLine.Text;

                    if (!dialogueLine.isQuestion)
                    {
                        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
                    }
                    else
                    {
                        playerAnswer = string.Empty;
                        DialogueSO nextDialogue = await DisplayChoicesAndAwaitInput(dialogueLine);

                        if (nextDialogue != null)
                        {
                            _currentDialogueBox = nextDialogue;
                            Tween tween = AnimateOut(currentRect, offsetSlidePosition);
                            text.text = string.Empty;
                            await UniTask.WaitUntil(() => !tween.IsPlaying());
                            break;
                        }
                        else
                        {
                            Tween tween = AnimateOut(currentRect, offsetSlidePosition);
                            text.text = string.Empty;
                            await UniTask.WaitUntil(() => !tween.IsPlaying());
                        }

                    }

                    Tween tweenOut = AnimateOut(currentRect, offsetSlidePosition);
                    text.text = string.Empty;
                    await UniTask.WaitUntil(() => !tweenOut.IsPlaying());
                    if (_currentDialogueBox.dialogues.IndexOf(dialogueLine) == _currentDialogueBox.dialogues.Count - 1)
                    {
                        _currentDialogueBox = null;
                        break;
                    }
                }
            }

            if (_currentDialogueBox != null && _currentDialogueBox.dialogues.Count > 0)
            {
                continue;
            }
            else
            {
                _currentDialogueBox = null;
            }
        }
    }


    private async UniTask<DialogueSO> DisplayChoicesAndAwaitInput(DialogueLine dialogueLine)
    {
        ClearChoices();

        foreach (var choice in dialogueLine.Choices)
        {
            GameObject newChoiceBox = Instantiate(_choiceBoxPrefab, _choicesGroup.transform);
            TextMeshProUGUI boxText = newChoiceBox.GetComponentInChildren<TextMeshProUGUI>();
            boxText.text = choice.ChoiceText;

            Button button = newChoiceBox.GetComponent<Button>();
            _choiceBoxes.Add(newChoiceBox);

        }

        return await AwaitChoiceSelection(dialogueLine);

    }

    private async UniTask<DialogueSO> AwaitChoiceSelection(DialogueLine dialogueLine)
    {
        var tcs = new UniTaskCompletionSource<DialogueSO>();

        foreach (var choiceBox in _choiceBoxes)
        {
            Button choiceButton = choiceBox.GetComponent<Button>();
            TextMeshProUGUI boxText = choiceBox.GetComponentInChildren<TextMeshProUGUI>(); 

            Choice currentChoice = dialogueLine.Choices.Find(c => c.ChoiceText == boxText.text);

            choiceButton.onClick.AddListener(() =>
            {
                DialogueSO selectedDialogue = currentChoice?.NextDialogue; 
                tcs.TrySetResult(selectedDialogue); 

                playerAnswer = currentChoice.ChoiceText;
                if (selectedDialogue == null) npcAnswer = currentChoice.AnswerText;

                ClearChoices();
            });
        }

        return await tcs.Task;
    }


    private void ClearChoices()
    {
        foreach (GameObject choiceBox in _choiceBoxes)
        {
            Destroy(choiceBox);
        }
        _choiceBoxes.Clear();
    }

    private RectTransform FindCharacter(Character character)
    {
        foreach (var dialogue in _characterDialogueImages)
        {
            string nameOfCharacter = dialogue.name.Replace(DialogueImage_Label, "");
            if (character.ToString() == nameOfCharacter)
            {
                return dialogue;
            }
        }
        return null;
    }

    private Vector2 SlideEffectSide(RectTransform rectTransform)
    {
        if (rectTransform.pivot.x == 0f)
        {
            return rightSlidePosition;
        }
        else if (rectTransform.pivot.x == 1f)
        {
            return leftSlidePosition;
        }
        return Vector2.zero;
    }


    private Tween AnimateIn(RectTransform rect, Vector2 targetPosition)
        => rect.DOAnchorPos(targetPosition, _slideEffectDuration).SetEase(Ease.OutQuad).SetUpdate(true);

    private Tween AnimateOut(RectTransform rect, Vector2 targetPosition)
        => rect.DOAnchorPos(targetPosition, _slideEffectDuration).SetEase(Ease.InQuad).SetUpdate(true);
}