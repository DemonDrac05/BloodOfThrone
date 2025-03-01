using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    #region Singleton Instance
    public GameObject playerInventoryCanvas;
    public static InventoryManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion


    [Header("=== CATEGORY: Inventory Slots ==========")]
    public List<InventoryCategoryConfig> InventoryConfigs = new();

    [Header("=== CATEGORY: Gear Slots ==========")]
    public List<GearSlot> GearSlots;

    [Header("=== CATEGORY: Stat Board ==========")]
    public List<TextMeshProUGUI> SumStatIndexes;
    public List<TextMeshProUGUI> AddStatIndexes;

    [Header("=== UI Elements ==========")]
    public GameObject inventoryItemPrefab;
    public GameObject itemBoardPrefab;
    public List<GameObject> itemBoards = new List<GameObject>();
    public Color slotSignalColor;

    [Header("#== Equipment Slots ==#")]
    public GameObject EquipmentFunctionBox;

    [Header("#== Bag Slots ==#")]
    public GameObject BagFunctionBox;

    // --- INVENTORY MAPS ----------
    // #-- BAG SLOTS  --#
    private Dictionary<GameObject, List<InventorySlot>> _inventorySlotsObjectsMap;
    private Dictionary<Button, GameObject> _inventoryButtonsMap;
    // #-- STAT BOARD --#
    private Dictionary<StatisticType, TextMeshProUGUI> _sumStatIndexesMap;
    private Dictionary<StatisticType, TextMeshProUGUI> _addStatIndexesMap;

    const string SumIndexLabel = "[Index-Sum] ";
    const string AddIndexLabel = "[Index-Add] ";

    private void OnEnable()
    {
        InitializeSlotsToCategory();
        InitializeInventoryMaps();

        foreach (Button button in _inventoryButtonsMap.Keys)
        {
            button.onClick.AddListener(() => ToggleInventoryButtons(button));
        }
    }

    private void OnDisable()
    {
        foreach (Button button in _inventoryButtonsMap.Keys)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    public void ToggleInventoryButtons(Button clickedButton)
    {
        if (clickedButton == null) return;

        foreach (KeyValuePair<Button, GameObject> pair in _inventoryButtonsMap)
        {
            if (pair.Key == clickedButton)
            {
                pair.Value.SetActive(true);
            }
            else
            {
                pair.Value.SetActive(false);
            }
        }
    }

    public Button GetButtonFromConfigs(InventorySlotType slotType)
    {
        foreach (var config in InventoryConfigs)
        {
            if (slotType == config.SlotType)
            {
                return config.SlotGroupButton;
            }
        }
        return null;
    }

    private void Start()
    {
        // #-- STAT BOARD --#
        InitializeStatIndexesMap();
    }

    #region === INITIALIZE MAPS ==========
    #region #== BAG SLOTS ==#
    private void InitializeSlotsToCategory()
    {
        foreach (var config in InventoryConfigs)
        {
            for (int i = 0; i < config.SlotGroup.transform.childCount; i++)
            {
                InventorySlot newSlot = config.SlotGroup.transform.GetChild(i).GetComponent<InventorySlot>();
                newSlot.InventorySlotType = config.SlotType;
                config.Slots.Add(newSlot);
            }
        }
    }

    private void InitializeInventoryMaps()
    {
        _inventoryButtonsMap = new Dictionary<Button, GameObject>();
        _inventorySlotsObjectsMap = new Dictionary<GameObject, List<InventorySlot>>();

        foreach (var config in InventoryConfigs)
        {
            _inventoryButtonsMap[config.SlotGroupButton] = config.SlotGroup;
            _inventorySlotsObjectsMap[config.SlotGroup] = config.Slots;
        } 
    }
    #endregion

    #region #== STAT BOARD ==#
    private void InitializeStatIndexesMap()
    {
        _sumStatIndexesMap = new Dictionary<StatisticType, TextMeshProUGUI>();
        _addStatIndexesMap = new Dictionary<StatisticType, TextMeshProUGUI>();

        InsertTextObjectsToStatMaps(SumStatIndexes, _sumStatIndexesMap, SumIndexLabel);
        InsertTextObjectsToStatMaps(AddStatIndexes, _addStatIndexesMap, AddIndexLabel);

    }

    private void InsertTextObjectsToStatMaps(List<TextMeshProUGUI> textMeshProUGUIs,
                                            Dictionary<StatisticType, TextMeshProUGUI> keyValuePairs,
                                            string label)
    {
        foreach (TextMeshProUGUI text in textMeshProUGUIs)
        {
            foreach(StatisticType statisticType in Enum.GetValues(typeof(StatisticType)))
            {
                string TMPObjectName = GUIManager.Instance.RemoveSpecificWord(text.name, label);
                if (TMPObjectName == statisticType.ToString())
                {
                    if (!keyValuePairs.ContainsValue(text))
                    {
                        keyValuePairs[statisticType] = text;
                        Debug.Log(text.name);
                        break;
                    }
                }
            }
        }
    }
    #endregion
    #endregion

    public void ResetRectPosition(RectTransform rectTransform)
    {
        Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out Vector2 localPoint))
        {
            rectTransform.localPosition = localPoint;
        }
    }

    #region === CHECK MOUSE-CLICK IN SLOT ==========
    public bool IsClickInsideAnyInventorySlot()
    {
        foreach (KeyValuePair<GameObject, List<InventorySlot>> pair in _inventorySlotsObjectsMap)
        {
            if (pair.Key.activeSelf)
            {
                if (IsClickInsideSlotObjects(pair.Value))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsClickInsideSlotObjects<T>(List<T> slots) where T : BaseSlot
    {
        foreach (var slot in slots)
        {
            RectTransform slotRect = slot.GetComponent<RectTransform>();
            if (IsClickInsideObject(slotRect))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsClickInsideObject(RectTransform rect)
    {
        Vector2 inputPosition = Input.mousePosition;
        Vector2 localClickPosition;

        RectTransform rectTransform = rect.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, inputPosition, null, out localClickPosition);
        return rectTransform.rect.Contains(localClickPosition);
    }
    #endregion

    #region === CHECK NO-ITEM IN SLOT ==========
    public InventorySlot CheckEmptySlotInInventorySlot()
    {
        foreach (KeyValuePair<GameObject, List<InventorySlot>> pair in _inventorySlotsObjectsMap)
        {
            if (pair.Key.activeSelf)
            {
                foreach (InventorySlot slot in pair.Value)
                {
                    if (ChecKEmptySlotInSlots(slot) != null)
                    {
                        return slot;
                    }
                }
            }
        }
        return null;
    }

    public GearSlot CheckEmptySlotInGearSlot(InventoryItem itemToMove)
    {
        CombatItem combatItem = itemToMove.item as CombatItem;
        if (combatItem != null)
        {
            foreach (GearSlot slot in GearSlots)
            {
                if (combatItem.CombatItemType == slot.CombatItemType && ChecKEmptySlotInSlots(slot))
                {
                    return slot;
                }
            }
        }
        return null;
    }

    public T ChecKEmptySlotInSlots<T>(T slot) where T : BaseSlot
    {
        return slot.GetComponentInChildren<InventoryItem>() == null ? slot : null;
    }
    #endregion

    #region === ITEM-ARRANGEMENT FUNCTIONS ==========
    //public bool AddItem<T>(T item) where T : BaseItem
    //{
    //    if (AddItemToSlotArray(item, MainInventorySlots))
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //private bool AddItemToSlotArray<T>(T item, BaseSlot[] slots) where T : BaseItem
    //{
    //    foreach (var slot in slots)
    //    {
    //        var itemInSlot = slot.GetComponentInChildren<InventoryItem>();
    //        if (itemInSlot != null && itemInSlot.GetItem<T>() == item && itemInSlot.count < GetMaxStackable(item))
    //        {
    //            itemInSlot.count++;
    //            itemInSlot.RefreshCount();
    //            return true;
    //        }
    //    }

    //    foreach (var slot in slots)
    //    {
    //        if (slot.GetComponentInChildren<InventoryItem>() == null)
    //        {
    //            SpawnNewItem(item, slot);
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //public void AddItems<T>(T[] items) where T : BaseItem
    //{
    //    foreach (var item in items)
    //    {
    //        AddItem(item);
    //    }
    //}

    //private void SpawnNewItem<T>(T item, BaseSlot slot) where T : BaseItem
    //{
    //    var newItemGameObject = Instantiate(inventoryItemPrefab, slot.transform);
    //    var inventoryItem = newItemGameObject.GetComponent<InventoryItem>();
    //    inventoryItem.InitialiseItem(item);
    //    newItemGameObject.SetActive(true);
    //}

    //private int GetMaxStackable<T>(T item) where T : BaseItem
    //{
    //    return item switch
    //    {
    //        _ => 999
    //    };
    //}

    //public void MirrorSlots(BaseSlot sourceSlot, BaseSlot targetSlot)
    //{
    //    if (targetSlot.transform.childCount > 0)
    //    {
    //        for (int i = targetSlot.transform.childCount - 1; i >= 0; i--)
    //        {
    //            Destroy(targetSlot.transform.GetChild(i).gameObject);
    //        }
    //    }
    //    if (sourceSlot.transform.childCount > 0)
    //    {
    //        var sourceItem = sourceSlot.GetComponentInChildren<InventoryItem>();

    //        if (sourceItem != null)
    //        {
    //            var newItem = Instantiate(sourceItem.gameObject, targetSlot.transform);
    //            newItem.GetComponent<InventoryItem>().InitialiseItem(sourceItem.GetItem<BaseItem>());
    //            newItem.GetComponent<InventoryItem>().count = sourceItem.count;
    //            newItem.GetComponent<InventoryItem>().RefreshCount();

    //            newItem.name = sourceItem.gameObject.name.Replace("(Clone)", "");

    //            newItem.SetActive(true);
    //        }
    //    }
    //}
    #endregion

    #region === ITEM BOARD ==========
    public void SpawnItemBoard(CombatItem combatItem, Vector2 pivotPos)
    {
        if (combatItem != null)
        {
            GameObject newBoard = Instantiate(itemBoardPrefab, playerInventoryCanvas.transform);
            itemBoards.Add(newBoard);

            RectTransform rectTransform = newBoard.GetComponent<RectTransform>();
            rectTransform.pivot = pivotPos;
            rectTransform.localPosition = pivotPos.x == 0f ? Vector3.zero : new Vector3(-10f, 0f);

            ItemBoard newItemBoard = newBoard.GetComponent<ItemBoard>();
            newItemBoard.Initialize(combatItem);
        }
    }

    public void DestroyItemBoards()
    {
        foreach (GameObject gameObject in itemBoards.ToList())
        {
            if (gameObject != null) Destroy(gameObject);
            itemBoards.Remove(gameObject);
        }
    }
    #endregion

    #region === STAT BOARD ==========
    public void ResetStatBoard()
    {
        PlayerStatistic playerStat = PlayerStatistic.Instance;
        foreach (var key in _sumStatIndexesMap.Keys)
        {
            float statValue = playerStat.GetSumStat(key);
            _sumStatIndexesMap[key].text = GetStatText(statValue, playerStat.HasMultiplyStatValue(key), false);
        }

        foreach (var key in _addStatIndexesMap.Keys)
        {
            float statValue = playerStat.GetAddStat(key);
            _addStatIndexesMap[key].text = statValue >= 0.01f ?
                GetStatText(statValue, playerStat.HasMultiplyStatValue(key), true) : string.Empty;
        }
    }

    private string GetStatText(float statValue, bool isPercentage, bool isAdditive)
    {
        if (isPercentage)
        {
            float newTextType = (float)Math.Round(statValue * 100, 2);
            return (isAdditive ? "+" : "") + $"{newTextType}%";
        }
        else
        {
            float roundedValue = (float)Math.Round(statValue, 0);
            return (isAdditive && roundedValue > 0f ? "+" : "") + roundedValue.ToString();
        }
    }
    #endregion
}

[System.Serializable]
public class InventoryCategoryConfig
{
    [HideInInspector] public string CategoryName;
    public GameObject SlotGroup;
    public Button SlotGroupButton;
    public InventorySlotType SlotType;
    [HideInInspector] public List<InventorySlot> Slots = new();
}