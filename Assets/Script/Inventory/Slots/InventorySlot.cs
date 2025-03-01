using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;

public class InventorySlot : BaseSlot
{
    public InventorySlotType InventorySlotType;

    private void Awake()
    {
        _slotImage = GetComponent<Image>();
    }

    private void Update()
    {
        HandleSlotColorChanges();
    }

    void HandleSlotColorChanges()
    {
        InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();

        // Toggle color whenever button get clicked
        if (gameObject.activeSelf && inventoryItem != null)
        {
            if (EquipmentButtonClicked())
            {
                PingPongSlotColor().Forget();
            }
        }
        if (!EquipmentButtonClicked())
        {
            DeactivateSlotColor().Forget();
        }

        // Check mouse-click outside slots -> return slot back to originality
        if (EquipmentButtonClicked() && Input.GetMouseButton(0))
        {
            if (!InventoryManager.Instance.IsClickInsideAnyInventorySlot())
            {
                EquipmentFunctionButtons.isChangeButtonClicked = false;
                EquipmentFunctionButtons.isCompareButtonClicked = false;

                DeactivateSlotColor().Forget();
            }
        }
    }

    bool EquipmentButtonClicked() 
        => EquipmentFunctionButtons.isChangeButtonClicked || EquipmentFunctionButtons.isCompareButtonClicked;

    public override void OnLeftClickWithEmptyCursor(InventoryItem itemInSlot)
    {
        if (EquipmentFunctionButtons.isChangeButtonClicked)
        {
            SwapItemsToGearSlot(itemInSlot);
            EquipmentFunctionButtons.isChangeButtonClicked = false;
        }
        else if (EquipmentFunctionButtons.isCompareButtonClicked)
        {
            CompareItemToItem(itemInSlot);
            EquipmentFunctionButtons.isCompareButtonClicked = false;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (itemInSlot != null)
                {
                    GearSlot gearSlot = InventoryManager.Instance.CheckEmptySlotInGearSlot(itemInSlot);
                    if (gearSlot != null)
                    {
                        SetItemToSlot(itemInSlot, gearSlot);
                        gearSlot.AddItemStats(itemInSlot);
                    }
                }
            }
            else
            {
                base.OnLeftClickWithEmptyCursor(itemInSlot);
            }
        }
    }

    private void SwapItemsToGearSlot(InventoryItem itemInInventorySlot)
    {
        if (itemInInventorySlot != null)
        {
            GearSlot currentGearSlot = EquipmentFunctionButtons.CurrentGearSlot;
            InventoryItem itemInGearSlot = currentGearSlot.GetComponentInChildren<InventoryItem>();

            Transform tempParent = itemInGearSlot.transform.parent;
            itemInGearSlot.parentAfterMove = itemInInventorySlot.transform.parent;
            itemInInventorySlot.parentAfterMove = tempParent;

            itemInGearSlot.transform.SetParent(itemInGearSlot.parentAfterMove);
            itemInInventorySlot.transform.SetParent(itemInInventorySlot.parentAfterMove);

            currentGearSlot.RemoveItemStats(itemInGearSlot);
            currentGearSlot.AddItemStats(itemInInventorySlot);
        }
    }

    private void CompareItemToItem(InventoryItem itemInInventorySlot)
    {
        GearSlot currentGearSlot = EquipmentFunctionButtons.CurrentGearSlot;

        CombatItem combatItemInInventorySlot = itemInInventorySlot.item as CombatItem;
        CombatItem combatItemInGearSlot = currentGearSlot.GetComponentInChildren<InventoryItem>().item as CombatItem;

        InventoryManager.Instance.SpawnItemBoard(combatItemInInventorySlot, new Vector2(0f, 0.5f));
        InventoryManager.Instance.SpawnItemBoard(combatItemInGearSlot, new Vector2(1f, 0.5f));
    }
}

public enum InventorySlotType
{
    Narrative,
    Amulet,
    Headwear,
    Jewelry,
    Pharmaceutical,
    Material,
    None
}