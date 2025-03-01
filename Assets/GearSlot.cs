using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

public class GearSlot : BaseSlot
{
    public CombatItemType CombatItemType;

    public override async void OnEnable()
    {
        await InitializeItemStats();
        SetGearSlotType();
    }

    private async UniTask InitializeItemStats()
    {
        PlayerStatistic playerStat = GetComponentInParent<PlayerStatistic>();
        await UniTask.WaitUntil(() => playerStat.CompleteDictImplement());

        InventoryItem itemInSlot = GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            AddItemStats(itemInSlot);
        }
    }

    private void SetGearSlotType()
    {
        if (CombatItemType == CombatItemType.Armory || CombatItemType == CombatItemType.Weapon)
        {
            SlotType = SlotType.Non_Clickable;
        }
        else
        {
            SlotType = SlotType.Clickable;
        }
    }

    private void Update()
    {
        InventoryItem currentItemOnCursor = InventoryItem.CurrentMovingItem;
        if (currentItemOnCursor != null)
        {
            CombatItem combatItem = currentItemOnCursor.item as CombatItem;
            if (combatItem != null && combatItem.CombatItemType == CombatItemType)
            {
                PingPongSlotColor().Forget();
            }
        }
        else
        {
            DeactivateSlotColor().Forget();
        }
        InventoryManager.Instance.ResetStatBoard();
    }

    public override void HandleMouseInput(PointerEventData eventData)
    {
        InventoryManager inventoryManager = InventoryManager.Instance;
        inventoryManager.ToggleInventoryButtons(CombatItemType switch
        {
            CombatItemType.Headwear => inventoryManager.HeadwearSlotGroupButton,
            CombatItemType.Jewelry => inventoryManager.JewelrySlotGroupButton,
            CombatItemType.Amulet => inventoryManager.AmuletSlotGroupButton,
            _ => null
        });

        base.HandleMouseInput(eventData);
    }

    #region --- LEFT CLICK ----------
    public override void OnLeftClickWithOccupiedCursor(InventoryItem itemInSlot)
    {
        
        InventoryItem inventoryItem = InventoryItem.CurrentMovingItem;
        if (inventoryItem.item is CombatItem)
        {
            CombatItem combatItem = (CombatItem)inventoryItem.item;
            if (combatItem.CombatItemType == CombatItemType)
            {
                base.OnLeftClickWithOccupiedCursor(itemInSlot);
            }
        }
    }

    public override void OnLeftClickWithEmptyCursor(InventoryItem itemInSlot)
    {
        if (itemInSlot != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                InventorySlot emptySlot = InventoryManager.Instance.CheckEmptySlotInInventorySlot();
                if (emptySlot != null)
                {
                    SetItemToSlot(itemInSlot, emptySlot);
                    RemoveItemStats(itemInSlot);
                }
            }
            else
            {
                base.OnLeftClickWithEmptyCursor(itemInSlot);
                RemoveItemStats(itemInSlot);
            }
        }
    }

    public override void DropItemWithLeftClick(InventoryItem inventoryItem)
    {
        base.DropItemWithLeftClick(inventoryItem);
        AddItemStats(inventoryItem);
    }

    public override void SwapItemsFromSlot(InventoryItem itemOnCursor, InventoryItem itemInSlot)
    {
        base.SwapItemsFromSlot(itemOnCursor, itemInSlot);
        RemoveItemStats(itemInSlot);
        AddItemStats(itemOnCursor);
    }


    public void AddItemStats(InventoryItem checkItem)
    {
        if (checkItem.item is CombatItem)
        {
            CombatItem item = (CombatItem)checkItem.item;

            PlayerStatistic playerStat = GetComponentInParent<PlayerStatistic>();
            if (item.Statistics.Count > 0)
            {
                foreach (var stat in item.Statistics)
                {
                    playerStat.AddStatValue(stat.Type, stat.ValueType, stat.Value);
                }
            }

        }
    }

    public void RemoveItemStats(InventoryItem checkItem)
    {
        if (checkItem.item is CombatItem)
        {
            CombatItem item = (CombatItem)checkItem.item;

            PlayerStatistic playerStat = GetComponentInParent<PlayerStatistic>();
            if (item.Statistics.Count > 0)
            {
                foreach (var stat in item.Statistics)
                {
                    playerStat.RemoveStatValue(stat.Type, stat.ValueType, stat.Value);
                }
            }

        }
    }
    #endregion

    #region --- RIGHT CLICK ----------

    public override void OnRightClickWithEmptyCursor(InventoryItem itemInSlot)
    {
        if (itemInSlot != null)
        {
            // Activate function board
            RectTransform functionbox = InventoryManager.Instance.EquipmentFunctionBox.GetComponent<RectTransform>();
            functionbox.gameObject.SetActive(true);
            InventoryManager.Instance.ResetRectPosition(functionbox);

            // Implement current gearslot to function board mememory
            EquipmentFunctionButtons.CurrentGearSlot = this;
        }
    }
    #endregion
}
