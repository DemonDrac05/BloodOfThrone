using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;


public class BaseSlot : MonoBehaviour, IPointerClickHandler
{
    protected InventoryItem cachedItem;
    protected SlotType SlotType;

    // --- GUI PROPERTIES ----------
    protected Color _originalSlotColor;
    protected Color _slotSignalColor;
    protected Image _slotImage;

    public virtual void OnEnable()
    {
        SlotType = SlotType.Clickable;
    }

    public virtual void Start()
    {
        _slotImage = GetComponent<Image>();
        _originalSlotColor = _slotImage.color;
        _slotSignalColor = InventoryManager.Instance.slotSignalColor;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (SlotType == SlotType.Clickable)
        {
            HandleMouseInput(eventData);
        }
    }

    public virtual void HandleMouseInput(PointerEventData eventData)
    {
        cachedItem = GetComponentInChildren<InventoryItem>();

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick(cachedItem);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick(cachedItem);
        }
    }

    #region === LEFT CLICK ==========
    public virtual void OnLeftClick(InventoryItem itemInSlot)
    {
        if (InventoryItem.CurrentMovingItem != null)
        {
            OnLeftClickWithOccupiedCursor(itemInSlot);
        }
        else
        {
            OnLeftClickWithEmptyCursor(itemInSlot);
        }
    }

    public virtual void OnLeftClickWithOccupiedCursor(InventoryItem itemInSlot)
    {
        InventoryItem inventoryItem = InventoryItem.CurrentMovingItem;
        if (itemInSlot == null)
        {
            DropItemWithLeftClick(inventoryItem);
        }
        else
        {
            SwapItemsFromSlot(inventoryItem, itemInSlot);
        }
    }

    public virtual void OnLeftClickWithEmptyCursor(InventoryItem itemInSlot)
    {
        if (itemInSlot != null)
        {
            PickUpItemWithLeftClick(itemInSlot);
        }
    }

    public virtual void PickUpItemWithLeftClick(InventoryItem itemInSlot)
    {
        itemInSlot.PickUpItem(itemInSlot);
    }

    public virtual void DropItemWithLeftClick(InventoryItem inventoryItem)
    {
        inventoryItem.parentAfterMove = transform;
        inventoryItem.DropItem(inventoryItem);
    }

    public virtual void SwapItemsFromSlot(InventoryItem itemOnCursor, InventoryItem itemInSlot)
    {
        itemOnCursor.SwapItems(itemOnCursor, itemInSlot);
    }
    #endregion

    #region === RIGHT CLICK ==========
    public virtual void OnRightClick(InventoryItem itemInSlot)
    {
        if (InventoryItem.CurrentMovingItem != null)
        {
            OnRightClickWithOccupiedCursor(itemInSlot);
        }
        else
        {
            OnRightClickWithEmptyCursor(itemInSlot);
        }
    }

    public virtual void OnRightClickWithOccupiedCursor(InventoryItem itemInSlot)
    {
        InventoryItem inventoryItem = InventoryItem.CurrentMovingItem;
        if (itemInSlot == null)
        {
            DropItemWithRightClick(inventoryItem);
        }
        else
        {
            MergeItemsWithRightClick(inventoryItem, itemInSlot);
            itemInSlot.RefreshCount();
        }
        RefreshInventory(inventoryItem);
    }

    public virtual void OnRightClickWithEmptyCursor(InventoryItem itemInSlot)
    {
        if (itemInSlot != null)
        {
            SplitItemWithRightClick(itemInSlot);
            itemInSlot.RefreshCount();
        }
    }

    public virtual void SplitItemWithRightClick(InventoryItem itemInSlot)
    {
        if (itemInSlot.count > 1)
        {
            InventoryItem newItem = CreateNewItem(transform, itemInSlot.GetItem<BaseItem>());

            newItem.count = itemInSlot.count / 2;
            itemInSlot.count -= newItem.count;

            newItem.RefreshCount();
            newItem.PickUpItem(newItem);
        }
    }

    public virtual void DropItemWithRightClick(InventoryItem inventoryItem)
    {
        InventoryItem newItem = CreateNewItem(transform, inventoryItem.GetItem<BaseItem>());
        inventoryItem.count--;
    }

    public virtual void MergeItemsWithRightClick(InventoryItem inventoryItem, InventoryItem itemInSlot)
    {
        if (inventoryItem.GetItem<BaseItem>() == itemInSlot.GetItem<BaseItem>())
        {
            itemInSlot.count++;
            inventoryItem.count--;
        }
    }

    public InventoryItem CreateNewItem(Transform parent, BaseItem baseItem)
    {
        GameObject newInventoryItem = InventoryManager.Instance.inventoryItemPrefab;
        GameObject copyItem = Instantiate(newInventoryItem, parent);
        InventoryItem newItem = copyItem.GetComponent<InventoryItem>();

        newItem.InitialiseItem(baseItem);
        return newItem;
    }

    public void RefreshInventory(InventoryItem inventoryItem)
    {
        inventoryItem.RefreshCount();

        if (inventoryItem.count == 0)
        {
            InventoryItem.CurrentMovingItem = null;
            Destroy(inventoryItem.gameObject);
        }
    }
    #endregion

    #region === SLOT FUNCTIONS ==========
    public virtual void SetItemToSlot<T>(InventoryItem itemInSlot, T slot) where T : BaseSlot
    {
        itemInSlot.parentAfterMove = slot.transform;
        itemInSlot.transform.SetParent(itemInSlot.parentAfterMove);
    }
    #endregion

    #region === GUI MANAGER ==========
    protected async UniTask PingPongSlotColor()
    {
        await GUIManager.Instance.PingPongColors(_slotImage, _originalSlotColor, _slotSignalColor, 1f);
    }

    protected async UniTask DeactivateSlotColor()
    {
        await GUIManager.Instance.TransferColorToColor(_slotImage, _originalSlotColor, 0.1f);
    }
    #endregion
}
public enum SlotType { Non_Clickable, Clickable }
