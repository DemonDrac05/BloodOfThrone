using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TypewriterEffect : MonoBehaviour
{
    public float charsPerSecond = 30;
    public string fullText;

    private TextMeshProUGUI textComponent;
    private int currentCharacterIndex = 0;
    private float timeSinceLastChar = 0;
    private bool isTyping = false;

    private void OnEnable()
    {
        if (textComponent != null)
        {
            textComponent = GetComponent<TextMeshProUGUI>();
        }
    }

    public void Initialize(TextMeshProUGUI textComponent)
    {
        this.textComponent = textComponent;
    }

    public async UniTask StartTyping(string text = "")
    {
        if (text != "")
        {
            fullText = text;
        }
        textComponent.text = "";
        currentCharacterIndex = 0;
        timeSinceLastChar = 0;
        isTyping = true;
        Debug.Log("it's preparing");
        await TypeEffect();
    }

    async UniTask TypeEffect()
    {
        while (isTyping)
        {

            Debug.Log("it's working");
            timeSinceLastChar += Time.deltaTime;

            if (timeSinceLastChar >= 1f / charsPerSecond)
            {
                timeSinceLastChar = 0;
                currentCharacterIndex++;

                if (currentCharacterIndex > fullText.Length)
                {
                    currentCharacterIndex = fullText.Length;
                    isTyping = false;
                }

                textComponent.text = fullText.Substring(0, currentCharacterIndex);

            }
            await UniTask.Yield();
        }

    }

    public bool IsTyping()
    {
        return isTyping;
    }
}