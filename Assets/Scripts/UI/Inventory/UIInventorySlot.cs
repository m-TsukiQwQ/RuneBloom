using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [Header("ToolbarSelection")]
    [SerializeField] private Sprite _selectedImage;
    private Sprite _originalImage;
    [SerializeField] private Image _bg;



    [Header("UI Slot Setup")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemStackSize;
    [SerializeField] private GameObject _itemToDrag;

    private UIInventory _inventoryUI;
    private Transform _originalParent; // To remember where the icon belongs


    private int _slotIndex;

    public void Select()
    {
        _bg.sprite = _selectedImage;
    }

    public void Deselect()
    {
        _bg.sprite = _originalImage;
    }

    private void Awake()
    {
        _originalImage = _bg.sprite;
        Deselect();

    }
    public void Init(int index, UIInventory inventory)
    {
        _slotIndex = index;
        _inventoryUI = inventory;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (_itemIcon.enabled == false) return;

        _originalParent = _itemToDrag.transform.parent;

        _itemToDrag.transform.SetParent(_inventoryUI.mainCanvas.transform);

        _itemIcon.raycastTarget = false;

        if (_inventoryUI.bgButton != null)
            _inventoryUI.bgButton.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_itemIcon.enabled == false) return;
        _itemToDrag.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Transform playerTransform = _inventoryUI.inventory.transform;
            Vector2 randomOffset = Random.insideUnitCircle * 1.5f;

            // 2. Add it to player position (Cast to Vector3 to match transform)
            Vector3 dropPos = playerTransform.position + (Vector3)randomOffset;

            // 3. Ensure Z axis stays at 0 (or same as player) to prevent sorting issues
            dropPos.z = playerTransform.position.z;

            _inventoryUI.inventory.DropItem(_slotIndex, dropPos);

        }
        if (_inventoryUI.bgButton != null)
            _inventoryUI.bgButton.raycastTarget = true;

        _itemToDrag.transform.SetParent(_originalParent);

        _itemToDrag.transform.localPosition = Vector3.zero;

        _itemIcon.raycastTarget = true;
    }
    public virtual void OnDrop(PointerEventData eventData)
    {
        // 1. Who is being dropped on me?
        GameObject droppedObj = eventData.pointerDrag;
        if (droppedObj == null) return;

        // 2. Get the slot script from that object
        UIInventorySlot incomingSlot = droppedObj.GetComponent<UIInventorySlot>();

        // 3. If it is a valid slot, tell the manager to swap our indices
        if (incomingSlot != null)
        {


            ItemDataSO incomingItem = incomingSlot.GetItem();

            // 2. Validate: Do I accept this item type?
            if (CanAccept(incomingItem))
            {

                ItemDataSO myItem = GetItem();
                if (myItem == null || incomingSlot.CanAccept(myItem))
                {
                    _inventoryUI.HandleSwap(incomingSlot.SlotIndex, this._slotIndex);
                }
            }
        }
    }

    public int SlotIndex => _slotIndex;

    public ItemDataSO GetItem()
    {
        // We need to ask the UI Manager for the data, or store a reference. 
        // For simplicity, let's just check the sprite or ask the system via index.
        return _inventoryUI.inventory.slots[_slotIndex].itemData;
    }

    public virtual bool CanAccept(ItemDataSO item)
    {
        return true; // Normal slots accept everything
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2 || eventData.clickCount == 1 && Input.GetKey(KeyCode.LeftShift))
        {
            if (GetItem() != null)
            {
                _inventoryUI.OnQuickEquip(_slotIndex, 1);
            }
        }
    }
}
