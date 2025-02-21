using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    [Header("=== UI Components ==========")]
    public Image image;
    public Text countText;

    // --- ITEM PROPERTIES ----------
    [SerializeField] public BaseItem item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterMove;
    [HideInInspector] public bool isMoving = false;

    // --- CANVAS COMPONENTES ----------
    private Canvas canvas;
    private RectTransform canvasRectTransform;
    private RectTransform rectTransform;

    public static InventoryItem CurrentMovingItem { get; set; }

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();

        isMoving = false;
    }

    private void Start()
    {
        if (item != null)
        {
            InitialiseItem(item);
            RefreshCount();
        }
    }

    public void InitialiseItem<T>(T newItem) where T : BaseItem
    {
        item = newItem;
        image.sprite = newItem.image;
        //ActivateSlider(newItem);
    }

    //private void ActivateSlider(ScriptableObject scriptableObject)
    //{
    //    if (scriptableObject is Tool toolItem)
    //    {
    //        slider.gameObject.SetActive(toolItem.actionType == ActionType.Water);
    //        if (toolItem.actionType == ActionType.Water)
    //        {
    //            tools.waterSlider = slider;
    //        }
    //    }
    //    else
    //    {
    //        slider.gameObject.SetActive(false);
    //    }
    //    RefreshCount();
    //}

    public void RefreshCount()
    {
        countText.text = count.ToString();
        countText.gameObject.SetActive(count > 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BaseSlot inventorySlot = GetComponentInParent<BaseSlot>();
        if (inventorySlot != null)
        {
            inventorySlot.OnPointerClick(eventData);
        }
    }

    public void PickUpItem(InventoryItem item)
    {
        item.isMoving = true;
        CurrentMovingItem = item;
        item.parentAfterMove = item.transform.parent;
        item.transform.SetParent(item.canvas.transform);
        item.image.raycastTarget = false;
    }

    public void DropItem(InventoryItem item)
    {
        item.isMoving = false;
        CurrentMovingItem = null;
        item.transform.SetParent(item.parentAfterMove);
        item.transform.localPosition = Vector3.zero;
        item.image.raycastTarget = true;
    }

    public void SwapItems(InventoryItem item1, InventoryItem item2)
    {
        if (item1.GetItem<ScriptableObject>() == item2.GetItem<ScriptableObject>() &&
            (item1.item.MaxStackable > 1 || item2.item.MaxStackable > 1))
        {
            if (item2.count < item2.item.MaxStackable)
            {
                bool canStackMore = item1.count + item2.count < item2.item.MaxStackable;
                if (canStackMore)
                {
                    item2.count += item1.count;
                    Destroy(item1.gameObject);
                }
                else
                {
                    item1.count -= (item2.item.MaxStackable - item2.count);
                    item2.count = item2.item.MaxStackable;
                }
                item1.RefreshCount();
                item2.RefreshCount();
            }
        }
        else
        {
            SwapParents(item1, item2);
            DropItem(item1);
            PickUpItem(item2);
        }
    }

    private void SwapParents(InventoryItem item1, InventoryItem item2)
    {
        Transform tempParent = item1.parentAfterMove;
        item1.parentAfterMove = item2.transform.parent;
        item2.parentAfterMove = tempParent;

        item1.transform.SetParent(item1.parentAfterMove);
        item2.transform.SetParent(item2.parentAfterMove);
    }

    private void Update()
    {
        if (isMoving)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out Vector2 localPoint))
            {
                rectTransform.sizeDelta = new(70f, 70f);
                rectTransform.localPosition = localPoint;
            }
        }
    }

    public T GetItem<T>() where T : ScriptableObject => item as T;
}
