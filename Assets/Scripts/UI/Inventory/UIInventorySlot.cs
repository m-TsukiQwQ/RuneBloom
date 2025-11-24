using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //public InventoryItem itemInSlot { get; private set; }


    [Header("UI Slot Setup")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemStackSize;
    [SerializeField] private GameObject _itemToDrag;
    private Transform _originalParent; // To remember where the icon belongs
    private Canvas _mainCanvas;        // To know where to float the icon

    private int _slotIndex;
    public void Init(int index, Canvas canvas)
    {
        _slotIndex = index;
        _mainCanvas = canvas;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_itemIcon.enabled == false) return;

        _originalParent = _itemToDrag.transform.parent;

        _itemToDrag.transform.SetParent(_mainCanvas.transform);

        _itemIcon.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_itemIcon.enabled == false) return;

        // Move the icon to the mouse position
        _itemToDrag.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _itemToDrag.transform.SetParent(_originalParent);

        _itemToDrag.transform.localPosition = Vector3.zero;

        _itemIcon.raycastTarget = false;
    }

    public void UpdateSlot(InventorySlot slot)
    {
        if (slot != null && slot.HasItem)
        {
            _itemIcon.sprite = slot.itemData.itemIcon;
            _itemIcon.color = Color.white;
            _itemIcon.enabled = true;

            if (slot.stackSize > 1)
            {
                _itemStackSize.text = slot.stackSize.ToString();
                _itemStackSize.enabled = true;
            }
            else
                _itemStackSize.enabled = false;

        }
        else
        {
            _itemIcon.sprite = null;
            _itemIcon.enabled = false;
            _itemStackSize.enabled = false;
        }

    }

   
}
