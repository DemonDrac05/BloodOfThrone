using UnityEngine;
using UnityEngine.UI;

public class SwapButton : MonoBehaviour
{
    public GameObject GameObject1;
    public GameObject GameObject2;

    private Button Button;

    private void OnEnable()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(ToggleObjects);
    }

    private void OnDisable()
    {
        Button.onClick.RemoveAllListeners();
    }

    void ToggleObjects()
    {
        if (GameObject1.activeSelf)
        {
            GameObject1.SetActive(false);
            GameObject2.SetActive(true);
        }
        else
        {
            GameObject1.SetActive(true);
            GameObject2.SetActive(false);
        }
    }
}
