using UnityEngine;
using UnityEngine.UI;

public class ButtonEffects : MonoBehaviour
{
    public Color enterColor;
    private Color exitColor;
    private Image Image;

    private void Awake() 
    {
        Image = GetComponent<Image>();
        exitColor = Image.color;
    }

    public void OnButtonEnter()
    {
        Image.color = enterColor;
    }

    public void OnButtonExit()
    {
        Image.color = exitColor;
    }
}
