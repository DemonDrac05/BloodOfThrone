using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemManager : MonoBehaviour
{
    [SerializeField] protected GameObject informationBoardPrefab;
    [SerializeField] protected GameObject layoutsGroup;
    private Canvas canvas;
    private RectTransform rectTransform;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI itemCost;
    private TextMeshProUGUI itemDescription;
    private TextMeshProUGUI itemMainUse;

    private const string TMP_Label = "[TMP] ";
    private const string ItemNameTMP = TMP_Label + "ItemName";
    private const string ItemCostTMP = TMP_Label + "ItemCost";
    private const string ItemDescriptionTMP = TMP_Label + "ItemDescription";
    private const string ItemMainUseTMP = TMP_Label + "ItemMainUse";

    private const string InformationBoard_Label = "[Information Board] ";

    private void OnEnable()
    {
    }

    public void HideInformationBoard(BaseItem item)
    {
        string nameOfInfoBoard = InformationBoard_Label + item.name;
        GameObject infoBoardObj = layoutsGroup.transform.Find(nameOfInfoBoard).gameObject;
        if (infoBoardObj != null)
        {
            DestroyImmediate(infoBoardObj);
        }
    }

    private void CreateNewInfoBoard(PointerEventData eventData, BaseItem item, string name)
    {
        GameObject newInfoBoard = Instantiate(informationBoardPrefab, layoutsGroup.transform);
        newInfoBoard.name = InformationBoard_Label + name;
        newInfoBoard.SetActive(false);

        canvas = layoutsGroup.GetComponentInParent<Canvas>();
        rectTransform = newInfoBoard.GetComponent<RectTransform>();

        itemName = newInfoBoard.transform.Find(ItemNameTMP).GetComponent<TextMeshProUGUI>();
        itemCost = newInfoBoard.transform.Find(ItemCostTMP).GetComponent<TextMeshProUGUI>();
        itemDescription = newInfoBoard.transform.Find(ItemDescriptionTMP).GetComponent<TextMeshProUGUI>();
        itemMainUse = newInfoBoard.transform.Find(ItemMainUseTMP).GetComponent<TextMeshProUGUI>();

        rectTransform.localPosition = UpdatePositionWithMouseCursor(eventData);

        itemName.text = item.name;
        itemName.color = SetTextColorByRarity(item.RarityType);

        if (item.sellingPrice < 0) itemCost.text = string.Empty;
        else itemCost.text = $"${item.sellingPrice.ToString()}";

        itemDescription.text = item.itemDescription != string.Empty
            ? item.itemDescription : string.Empty;
        itemMainUse.text = item.itemMainUse != string.Empty
            ? item.itemMainUse : string.Empty;

        newInfoBoard.SetActive(true);
    }

    public void ShowInformationBoard(PointerEventData eventData, BaseItem item)
    {
        CreateNewInfoBoard(eventData, item, item.name);
    }

    private Vector2 UpdatePositionWithMouseCursor(PointerEventData eventData)
    {
        Vector2 mousePosition = eventData.position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPosition
        );
        return localPosition;
    }

    public Color32 SetTextColorByRarity(RarityType rarityType)
    {
        Color32 color = rarityType switch
        {
            RarityType.Uncommon => new(0, 110, 0, 255),
            RarityType.Epic => new(0, 100, 175, 255),
            RarityType.Rare => new(70, 0, 150, 255),
            RarityType.Legendary => new(150, 75, 0, 255),
            RarityType.Mythical => new(150, 0, 20, 255),
            _ => new(255, 255, 255, 255)
        };
        return color;
    }
}
