using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using System.Linq;
using TMPro;
using System;

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

    #region Item Category Strings
    private const string NarrativeCategoryName = "Narrative";
    private const string AmuletCategoryName = "Amulet";
    private const string HeadwearCategoryName = "Headwear";
    private const string JewelryCategoryName = "Jewelry";
    private const string PharmaceuticalCategoryName = "Pharmaceutical";
    private const string MaterialCategoryName = "Material";

    #endregion
    [Header("=== Initial Items ==========")]

    [Header("=== CATEGORY: Inventory Slots ==========")]
    [Header("#== GameObject ==#")]
    public GameObject NarrativeSlotGroup;
    public GameObject AmuletSlotGroup;
    public GameObject HeadwearSlotGroup;
    public GameObject JewelrySlotGroup;
    public GameObject PharmaceuticalSlotGroup;
    public GameObject MaterialSlotGroup;

    [Header("#== Slots ==#")]
    [HideInInspector] public List<InventorySlot> NarrativeSlots = new();
    [HideInInspector] public List<InventorySlot> AmuletSlots = new();
    [HideInInspector] public List<InventorySlot> HeadwearSlots = new();
    [HideInInspector] public List<InventorySlot> JewelrySlots = new();
    [HideInInspector] public List<InventorySlot> PharmaceuticalSlots = new();
    [HideInInspector] public List<InventorySlot> MaterialSlots = new();

    [Header("#== Button ==#")]
    public Button NarrativeSlotGroupButton;
    public Button AmuletSlotGroupButton;
    public Button HeadwearSlotGroupButton;
    public Button JewelrySlotGroupButton;
    public Button PharmaceuticalSlotGroupButton;
    public Button MaterialSlotGroupButton;

    [Header("=== CATEGORY: Gear Slots ==========")]
    public List<GearSlot> GearSlots;

    [Header("=== CATEGORY: Stat Board ==========")]
    public List<TextMeshProUGUI> SumStatIndexes;
    public List<TextMeshProUGUI> AddStatIndexes;

    [Header("=== UI Elements ==========")]
    [Header("#== Prefabs ==#")]
    public GameObject inventoryItemPrefab;

    [Header("#== Item Board ==#")]
    public GameObject itemBoardPrefab;
    public List<GameObject> itemBoards = new List<GameObject>();

    [Header("=== Inventory Properties ==========")]
    public Color slotSignalColor;

    [Header("#== Equipment Slots ==#")]
    public GameObject EquipmentFunctionBox;

    [Header("#== Bag Slots ==#")]
    public GameObject BagFunctionBox;

    // --- CATEGORY MAPS ----------
    // #-- BAG SLOTS --#
    private Dictionary<GameObject, List<InventorySlot>> _inventorySlotsObjectsMap;
    private Dictionary<string, List<InventorySlot>> _inventorySlotsCategoriesMap;
    private Dictionary<Button, GameObject> _inventoryButtonsMap;

    // #-- STAT BOARD --#
    private Dictionary<StatisticType, TextMeshProUGUI> _sumStatIndexesMap;
    private Dictionary<StatisticType, TextMeshProUGUI> _addStatIndexesMap;

    const string SumIndexLabel = "[Index-Sum] ";
    const string AddIndexLabel = "[Index-Add] ";

    private void OnEnable()
    {
        InitializeInventoryButtonsMap();

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

    private void Start()
    {
        // #-- BAG SLOTS --#
        InitializeInventorySlotsCategoriesMap();
        InitializeInventorySlotsObjectsMap();

        // #-- STAT BOARD --#
        InitializeStatIndexesMap();
        InsertTextObjectsToMap(SumStatIndexes, _sumStatIndexesMap, SumIndexLabel);
        InsertTextObjectsToMap(AddStatIndexes, _addStatIndexesMap, AddIndexLabel);
    }

    #region === INITIALIZE MAPS ==========
    #region #== BAG SLOTS ==#
    private void InitializeSlotsByGroup(string key, GameObject groupObjects)
    {
        var slots = _inventorySlotsCategoriesMap[key];
        if (slots == null) return;

        slots.Clear();
        for (int i = 0; i < groupObjects.transform.childCount; i++)
        {
            Transform child = groupObjects.transform.GetChild(i);
            InventorySlot newSlot = child.GetComponent<InventorySlot>();
            if (newSlot != null)
            {
                newSlot.InventorySlotType = key switch
                {
                    NarrativeCategoryName => InventorySlotType.Narrative,
                    AmuletCategoryName => InventorySlotType.Amulet,
                    HeadwearCategoryName => InventorySlotType.Headwear,
                    JewelryCategoryName => InventorySlotType.Jewelry,
                    PharmaceuticalCategoryName => InventorySlotType.Pharmaceutical,
                    _ => InventorySlotType.Material
                };
                slots.Add(newSlot);
            }
        }
    }

    private void InitializeInventoryButtonsMap()
    {
        _inventoryButtonsMap = new Dictionary<Button, GameObject>
        {
            { NarrativeSlotGroupButton, NarrativeSlotGroup },
            { AmuletSlotGroupButton, AmuletSlotGroup },
            { HeadwearSlotGroupButton, HeadwearSlotGroup },
            { JewelrySlotGroupButton, JewelrySlotGroup },
            { PharmaceuticalSlotGroupButton, PharmaceuticalSlotGroup },
            { MaterialSlotGroupButton, MaterialSlotGroup }
        };
    }

    private void InitializeInventorySlotsCategoriesMap()
    {
        _inventorySlotsCategoriesMap = new Dictionary<string, List<InventorySlot>>
        {
            { NarrativeCategoryName, NarrativeSlots},
            { AmuletCategoryName,AmuletSlots },
            { HeadwearCategoryName, HeadwearSlots },
            { JewelryCategoryName, JewelrySlots },
            { PharmaceuticalCategoryName, PharmaceuticalSlots },
            { MaterialCategoryName, MaterialSlots }
        };

        InitializeSlotsByGroup(NarrativeCategoryName, NarrativeSlotGroup);
        InitializeSlotsByGroup(AmuletCategoryName, AmuletSlotGroup);
        InitializeSlotsByGroup(HeadwearCategoryName, HeadwearSlotGroup);
        InitializeSlotsByGroup(JewelryCategoryName, JewelrySlotGroup);
        InitializeSlotsByGroup(PharmaceuticalCategoryName, PharmaceuticalSlotGroup);
        InitializeSlotsByGroup(MaterialCategoryName, MaterialSlotGroup);
    }

    private void InitializeInventorySlotsObjectsMap()
    {
        _inventorySlotsObjectsMap = new Dictionary<GameObject, List<InventorySlot>>
        {
            { NarrativeSlotGroup, NarrativeSlots},
            { AmuletSlotGroup,AmuletSlots },
            { HeadwearSlotGroup, HeadwearSlots },
            { JewelrySlotGroup, JewelrySlots },
            { PharmaceuticalSlotGroup, PharmaceuticalSlots },
            { MaterialSlotGroup, MaterialSlots }
        };
    }
    #endregion

    #region #== STAT BOARD ==#
    private void InitializeStatIndexesMap()
    {
        TextMeshProUGUI newText = new TextMeshProUGUI();
        _sumStatIndexesMap = new Dictionary<StatisticType, TextMeshProUGUI>
        {
            {StatisticType.MaxHealth,        newText},
            {StatisticType.HealthRecover,    newText},
            {StatisticType.PhysicalDamage,   newText},
            {StatisticType.PhysicalDefense,  newText},
            {StatisticType.PhysicalPiercing, newText},
            {StatisticType.MagicalDamage,    newText},
            {StatisticType.MagicalDefense,   newText},
            {StatisticType.MagicalPiercing,  newText},
            {StatisticType.CriticalChance,   newText},
            {StatisticType.CriticalDamage,   newText},
            {StatisticType.OmniVampChance,   newText},
            {StatisticType.OmniVampValue,    newText},
        };

        _addStatIndexesMap = new Dictionary<StatisticType, TextMeshProUGUI>
        {
            {StatisticType.MaxHealth,        newText},
            {StatisticType.HealthRecover,    newText},
            {StatisticType.PhysicalDamage,   newText},
            {StatisticType.PhysicalDefense,  newText},
            {StatisticType.PhysicalPiercing, newText},
            {StatisticType.MagicalDamage,    newText},
            {StatisticType.MagicalDefense,   newText},
            {StatisticType.MagicalPiercing,  newText},
            {StatisticType.CriticalChance,   newText},
            {StatisticType.CriticalDamage,   newText},
            {StatisticType.OmniVampChance,   newText},
            {StatisticType.OmniVampValue,    newText},
        };
    }

    private void InsertTextObjectsToMap(List<TextMeshProUGUI> textMeshProUGUIs,
                                            Dictionary<StatisticType, TextMeshProUGUI> keyValuePairs,
                                            string label)
    {
        foreach (var type in textMeshProUGUIs)
        {
            foreach(StatisticType statisticType in keyValuePairs.Keys)
            {
                string TMPObjectName = GUIManager.Instance.RemoveSpecificWord(type.name, label);
                if (TMPObjectName == statisticType.ToString())
                {
                    if (!keyValuePairs.ContainsValue(type))
                    {
                        keyValuePairs[statisticType] = type;
                        Debug.Log(type.name);
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
                List<InventorySlot> slots = pair.Value;
                foreach (InventorySlot slot in slots)
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
                if (combatItem.CombatItemType == slot.CombatItemType)
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

    public T ChecKEmptySlotInSlots<T>(T slot) where T : BaseSlot
    {
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot == null)
        {
            return slot;
        }
        return null;
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
            string statIndexText;

            if (playerStat.HasMultiplyStatValue(key))
            {
                float newTextType = (float)Math.Round(statValue * 100, 2);
                statIndexText = $"{newTextType}%";
            }
            else
            {
                statIndexText = Math.Round(statValue, 0).ToString();
            }
            _sumStatIndexesMap[key].text = statIndexText;
        }

        foreach (var key in _addStatIndexesMap.Keys)
        {
            float statValue = playerStat.GetAddStat(key);
            string statIndexText;

            if (statValue > 0)
            {
                if (playerStat.HasMultiplyStatValue(key))
                {
                    float newTextType = (float)Math.Round(statValue * 100, 2);
                    statIndexText = $"+{newTextType}%";
                }
                else
                {
                    float newTextType = (float)Math.Round(statValue, 0);
                    statIndexText = $"+{newTextType}";
                }
            }
            else
            {
                statIndexText = string.Empty;
            }
            _addStatIndexesMap[key].text = statIndexText;
        }
    }
    #endregion
}