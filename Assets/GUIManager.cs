using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }


    public async UniTask TransferColorToColor(Image targetImage, Color targetColor, float duration)
    {
        float elapsedTime = 0f;
        Color originColor = targetImage.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / duration);

            Color newColor = Color.Lerp(originColor, targetColor, progress);
            
            targetImage.color = newColor;

            await UniTask.Yield();
        }
        targetImage.color = targetColor;
    }

    public async UniTask PingPongColors(Image targetImage, Color originColor, Color targetColor, float duration)
    {
        if (targetImage.color == originColor)
        {
            await TransferColorToColor(targetImage, targetColor, duration);
            await UniTask.Delay(10);
        }
        else if (targetImage.color == targetColor)
        {
            await TransferColorToColor(targetImage, originColor, duration);
            await UniTask.Delay(10);
        }
    }

    public string SeparateCamelCase(string text)
    {
        return Regex.Replace(text, "([A-Z])", " $1").Trim();
    }

    public string RemoveSpecificWord(string originalString, string wordToRemove)
    {
        string newString = originalString.Replace(wordToRemove, "");

        newString = newString.Trim();
        while (newString.Contains("  "))
        {
            newString = newString.Replace("  ", " ");
        }

        return newString;
    }

    public Color SetColorByItemRarity(CombatItem combatItem)
    {
        Color color = combatItem.RarityType switch
        {
            RarityType.Uncommon => Color.green,
            RarityType.Rare => Color.blue,
            RarityType.Epic => Color.magenta,
            RarityType.Legendary => Color.yellow,
            RarityType.Mythical => Color.red,
            _ => Color.white
        };
        return color;
    }
}
