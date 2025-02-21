using TMPro;
using GifImporter;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class SSM : MonoBehaviour
{
    public GameObject title;
    public GameObject description;

    [TextArea] public string fullTextContent1;
    public TextMeshProUGUI content1;

    [TextArea] public string fullTextContent2;
    public TextMeshProUGUI content2;

    [TextArea] public string fullTextContent3;
    public TextMeshProUGUI content3;

    [TextArea] public string fullTextContent4;
    public TextMeshProUGUI content4;

    public GameObject scene1;
    public GameObject scene2;
    public Gif scene2Gif;

    public GameObject dialogue;
    public Image foreGround;
    public float transitionDuration;
    public float delayDuration;

    private TypewriterEffect typewriterEffect1;
    private TypewriterEffect typewriterEffect2;
    private TypewriterEffect typewriterEffect3;
    private TypewriterEffect typewriterEffect4;


    [Header("=== CHARACTERS' DIALOGUES ==========")]
    public SSDialogueSO firstDialogue;
    public SSDialogueSO secondDialogue;

    public GameObject DialogueBackground;
    public GameObject EvelynDialogue;
    public GameObject RaspitolDialogue;


    private async void Start()
    {
        typewriterEffect1 = content1.GetComponent<TypewriterEffect>();
        if (typewriterEffect1 == null)
        {
            typewriterEffect1 = content1.gameObject.AddComponent<TypewriterEffect>();
        }

        typewriterEffect2 = content2.GetComponent<TypewriterEffect>();
        if (typewriterEffect2 == null)
        {
            typewriterEffect2 = content2.gameObject.AddComponent<TypewriterEffect>();
        }
        typewriterEffect3 = content3.GetComponent<TypewriterEffect>();
        if (typewriterEffect3 == null)
        {
            typewriterEffect3 = content3.gameObject.AddComponent<TypewriterEffect>();
        }

        typewriterEffect4 = content4.GetComponent<TypewriterEffect>();
        if (typewriterEffect4 == null)
        {
            typewriterEffect4 = content4.gameObject.AddComponent<TypewriterEffect>();
        }

        typewriterEffect1.Initialize(content1); 
        typewriterEffect2.Initialize(content2); 
        typewriterEffect3.Initialize(content3);
        typewriterEffect4.Initialize(content4);

        await SSProcess();
    }

    async UniTask SSProcess()
    {
        await UniTask.WaitForSeconds(2f);

        await TransitionAlpha(foreGround, 1f);
        title.SetActive(false);
        description.SetActive(true);
        await TransitionAlpha(foreGround, 0f);

        await TransitionAlpha(foreGround, 1f);
        description.SetActive(false);
        await TransitionAlpha(foreGround, 0f);

        content1.gameObject.SetActive(true);
        await typewriterEffect1.StartTyping(fullTextContent1);
        await UniTask.WaitForSeconds(delayDuration);

        await TransitionAlpha(foreGround, 1f);
        content1.gameObject.SetActive(false);
        await TransitionAlpha(foreGround, 0f);

        content2.gameObject.SetActive(true);
        await typewriterEffect2.StartTyping(fullTextContent2);
        await UniTask.WaitForSeconds(delayDuration);

        await TransitionAlpha(foreGround, 1f);
        content2.gameObject.SetActive(false);
        scene1.SetActive(true);
        await TransitionAlpha(foreGround, 0f);

        await UniTask.WaitForSeconds(2f);
        DialogueBackground.SetActive(true);
        await UniTask.WaitUntil(() => DialogueBackground.activeSelf);

        await DialogueProcess(firstDialogue);
        DialogueBackground.SetActive(false);

        await TransitionAlpha(foreGround, 1f);
        scene1.gameObject.SetActive(false);
        await TransitionAlpha(foreGround, 0f);

        content3.gameObject.SetActive(true);
        await typewriterEffect3.StartTyping(fullTextContent3);

        await TransitionAlpha(foreGround, 1f);
        content3.gameObject.SetActive(false);
        scene2.gameObject.SetActive(true);
        await TransitionAlpha(foreGround, 0f);
        await SceneAnimated(scene2.GetComponent<Image>());

        await UniTask.WaitForSeconds(2f);
        DialogueBackground.SetActive(true);
        await UniTask.WaitUntil(() => DialogueBackground.activeSelf);

        await DialogueProcess(secondDialogue);
        DialogueBackground.SetActive(false);

        await TransitionAlpha(foreGround, 1f);
        scene2.gameObject.SetActive(false);
        scene1.gameObject.SetActive(true);
        await TransitionAlpha(foreGround, 0f);

        await TransitionAlpha(foreGround, 1f);
        scene1.gameObject.SetActive(false);
        await TransitionAlpha(foreGround, 0f);

        content4.gameObject.SetActive(true);
        await typewriterEffect4.StartTyping(fullTextContent4);

        await TransitionAlpha(foreGround, 1f);
        content4.gameObject.SetActive(false);
    }

    async UniTask DialogueProcess(SSDialogueSO sSDialogueSO)
    {
        foreach(var dialogue in sSDialogueSO.SSDialogueLines)
        {
            List<GameObject> dialogueObjs = new List<GameObject>() { EvelynDialogue, RaspitolDialogue };
            foreach (var dialogueObj in  dialogueObjs)
            {
                string characterNameObj = dialogueObj.name.Replace("[Dialogue] ", "");
                if (dialogue.Character.ToString() == characterNameObj)
                {
                    dialogueObj.SetActive(true); 

                    TextMeshProUGUI textMeshProUGUI = dialogueObj.GetComponentInChildren<TextMeshProUGUI>();
                    dialogueObj.GetComponentInChildren<TypewriterEffect>().Initialize(textMeshProUGUI);
                    await dialogueObj.GetComponentInChildren<TypewriterEffect>().StartTyping(dialogue.Text);
                    await UniTask.WaitForSeconds(delayDuration);
                    dialogueObj.SetActive(false);
                }
            }
        }
    }

    async UniTask TransitionAlpha(Image targetImage, float targetAlpha)
    {
        float progress = 0f;
        float startAlpha = targetImage.color.a;

        while (progress < 1f)
        {
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, progress);
            Color newColor = targetImage.color;
            newColor.a = currentAlpha;
            targetImage.color = newColor;

            progress += Time.deltaTime / transitionDuration;
            await UniTask.Yield();
        }

        Color finalColor = targetImage.color;
        finalColor.a = targetAlpha;
        targetImage.color = finalColor;
    }

    async UniTask SceneAnimated(Image image)
    {
        if (scene2.GetComponent<GifPlayer>() == null)
        {
            GifPlayer gifPlayer = scene2.AddComponent<GifPlayer>();
            gifPlayer.Gif = scene2Gif;
        }

        GifPlayer nameGifPlayer = scene2.GetComponent<GifPlayer>();
        nameGifPlayer.Activate();

        await UniTask.WaitUntil(() =>
            nameGifPlayer != null &&
            nameGifPlayer.Gif != null &&
            nameGifPlayer.Gif.Frames != null &&
            nameGifPlayer._index == nameGifPlayer.Gif.Frames.Count - 1
        );

        nameGifPlayer.loop = false;
    }
}