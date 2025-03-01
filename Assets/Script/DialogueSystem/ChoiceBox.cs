using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceBox : MonoBehaviour, IPointerClickHandler
{
    public bool onChoice;

    private Button _choiceButton;

    private void OnEnable()
    {
        _choiceButton = GetComponent<Button>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onChoice = true;
    }
}
