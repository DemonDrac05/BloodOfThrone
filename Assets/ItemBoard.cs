using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoard : MonoBehaviour
{
    [Header("=== UPPER GROUP COMPONENTS ==========")]
    private GameObject UpperGroupRect;

    private Image ItemImage;
    private TextMeshProUGUI ItemName;
    private TextMeshProUGUI ItemDescription;

    private TextMeshProUGUI SlotTypeFill;
    private TextMeshProUGUI RarityTypeFill;

    const string UpperGroup = "[Group-Upper] Overview";

    const string ItemImageObject = "[Image] Item";
    const string ItemNameObject  = "[TMP] ItemName";
    const string ItemDescriptionObject = "[TMP] ItemDescription";

    const string SlotTypeObject   = "[TMP] SlotType";
    const string RarityTypeObject = "[TMP] RarityType";

    [Header("=== LOWER GROUP COMPONENTS ==========")]
    private GameObject LowerGroupRect;

    [SerializeField] private GameObject StatIconObject;
    [SerializeField] private GameObject StatIndexObject;
    [SerializeField] private GameObject StatNameObject;

    private GameObject StatIconContainer;
    private GameObject StatIndexContainer;
    private GameObject StatNameContainer;

    const string LowerGroup = "[Group-Lower] Statistic";

    const string StatIconGroup  = "[Group] ItemStatisticIcon";
    const string StatNameGroup  = "[Group] ItemStatisticName";
    const string StatIndexGroup = "[Group] ItemStatisticIndex";

    private void OnEnable()
    {
        FindUpperGroupComponents();
        FindLowerGroupComponents();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && gameObject.activeSelf && !ClickedOnBoards())
        {
            InventoryManager.Instance.DestroyItemBoards();
        }
    }

    bool ClickedOnBoards()
    {
        InventoryManager inventoryManager = InventoryManager.Instance;
        foreach (GameObject gameObject in inventoryManager.itemBoards)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            if (inventoryManager.IsClickInsideObject(rectTransform))
            {
                return true;
            }
        }
        return false;
    }

    void FindUpperGroupComponents()
    {
        UpperGroupRect = transform.Find(UpperGroup).gameObject;

        ItemImage = UpperGroupRect.transform.Find(ItemImageObject).GetComponent<Image>();
        ItemName  = UpperGroupRect.transform.Find(ItemNameObject).GetComponent<TextMeshProUGUI>();
        ItemDescription = UpperGroupRect.transform.Find(ItemDescriptionObject).GetComponent<TextMeshProUGUI>();

        SlotTypeFill   = UpperGroupRect.transform.Find(SlotTypeObject).GetComponent<TextMeshProUGUI>();
        RarityTypeFill = UpperGroupRect.transform.Find(RarityTypeObject).GetComponent<TextMeshProUGUI>();
    }

    void FindLowerGroupComponents()
    {
        LowerGroupRect = transform.Find(LowerGroup).gameObject;

        StatIconContainer = LowerGroupRect.transform.Find(StatIconGroup).gameObject;
        StatNameContainer = LowerGroupRect.transform.Find(StatNameGroup).gameObject;
        StatIndexContainer = LowerGroupRect.transform.Find(StatIndexGroup).gameObject;
    }

    public void Initialize(CombatItem combatItem)
    {
        if (combatItem != null)
        {
            InitializeUpperGroup(combatItem);
            InitializeLowerGroup(combatItem);
        }
    }

    private void InitializeUpperGroup(CombatItem combatItem)
    {
        ItemImage.sprite = combatItem.image;

        ItemName.text = combatItem.name;
        ItemName.color = GUIManager.Instance.SetColorByItemRarity(combatItem);

        ItemDescription.text = combatItem.itemDescription;

        SlotTypeFill.text = combatItem.CombatItemType.ToString();

        RarityTypeFill.text = combatItem.RarityType.ToString();
        RarityTypeFill.color = GUIManager.Instance.SetColorByItemRarity(combatItem);
    }

    private void InitializeLowerGroup(CombatItem combatItem)
    {
        foreach (var stat in combatItem.Statistics)
        {
            // === ICON BLOCK ==========
            GameObject newStatIcon = Instantiate(StatIconObject, StatIconContainer.transform);

            // === INDEX BLOCK ==========
            GameObject newStatIndex = Instantiate(StatIndexObject, StatIndexContainer.transform);
            string statValueText = stat.ValueType switch
            {
                ValueType.Multiply => $"+{stat.Value * 100}%",
                ValueType.Additive => $"+{stat.Value * 100}",
                _ => string.Empty
            };

            TextMeshProUGUI statIndexText = newStatIndex.GetComponent<TextMeshProUGUI>();
            statIndexText.text = statValueText;

            // === NAME BLOCK ==========
            GameObject newStatName = Instantiate(StatNameObject, StatNameContainer.transform);
            string statTypeText = GUIManager.Instance.SeparateCamelCase(stat.Type.ToString());

            TextMeshProUGUI statNameText = newStatName.GetComponent<TextMeshProUGUI>();
            statNameText.text = statTypeText;
        }
    }
}
