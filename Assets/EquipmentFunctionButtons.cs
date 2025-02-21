using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentFunctionButtons : MonoBehaviour
{
    private Button ChangeButton;
    private Button CompareButton;

    const string ButtonLabel = "[Button] ";
    const string ChangeButtonLabel = ButtonLabel + "Change";
    const string CompareButtonLabel = ButtonLabel + "Compare";

    public static bool isChangeButtonClicked = false;
    public static bool isCompareButtonClicked = false;
    private bool _isObjectClicked = false;

    public static GearSlot CurrentGearSlot;

    public static EquipmentFunctionButtons Instance { get; private set; }
    private void Awake() => Instance = this;

    private void OnEnable()
    {
        ChangeButton = transform.Find(ChangeButtonLabel).GetComponent<Button>();
        CompareButton = transform.Find(CompareButtonLabel).GetComponent<Button>();

        ChangeButton.onClick.AddListener(OnChangeButtonClicked);
        CompareButton.onClick.AddListener(OnCompareButtonClicked);
    }

    private void OnDisable()
    {
        ChangeButton.onClick.RemoveAllListeners();
        CompareButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        isChangeButtonClicked = false;
        isCompareButtonClicked = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameObject.activeSelf && !IsClickInsideObject())
        {
            gameObject.SetActive(false);
            CurrentGearSlot = null;
        }
    }

    private bool IsClickInsideObject()
    {
        Vector2 inputPosition = Input.mousePosition;
        Vector2 localClickPosition;

        RectTransform rectTransform = GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, inputPosition, null, out localClickPosition);
        return rectTransform.rect.Contains(localClickPosition);
    }

    void OnChangeButtonClicked()
    {
        isChangeButtonClicked = true;
        gameObject.SetActive(false);
    }

    void OnCompareButtonClicked()
    {
        isCompareButtonClicked = true;
        gameObject.SetActive(false);
    }
}
