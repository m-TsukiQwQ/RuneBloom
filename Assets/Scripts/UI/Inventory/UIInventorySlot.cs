using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;


public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("ToolbarSelection")]
    [SerializeField] private Sprite _selectedImage;
    private Sprite _originalImage;
    [SerializeField] private Image _bg;



    [Header("UI Slot Setup")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemStackSize;
    [SerializeField] private GameObject _itemToDrag;

    private GameObject _ghostIconObj; // The temporary icon we create for right-click
    private bool _isSplitDrag;        // Are we dragging half or whole?
    private int _dragAmount;          // How many are we dragging?


    private UIInventory _inventoryUI;
    private Transform _originalParent; // To remember where the icon belongs
    [SerializeField] private UIManager _ui;

    private int _slotIndex;
    // Public getter so the Drop Target knows if we are splitting
    public bool IsSplitDrag => _isSplitDrag;
    public int DragAmount => _dragAmount;

    private ItemDataSO ItemData()
    {
        return _inventoryUI.inventory.slots[SlotIndex].itemData;
    }

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
    private void Start()
    {
        _ui = GetComponentInParent<UIManager>(true);
    }
    public void Init(int index, UIInventory inventory)
    {
        _slotIndex = index;
        _inventoryUI = inventory;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _ui.ShowItemToolTip(false);

        if (_itemIcon.enabled == false) return;
        InventorySlot slotData = _inventoryUI.inventory.slots[_slotIndex];

        if (eventData.button == PointerEventData.InputButton.Right && slotData.stackSize > 1)
        {
            _isSplitDrag = true;
            _dragAmount = Mathf.CeilToInt(slotData.stackSize / 2f); // Take half (rounded up)

            // Create a Ghost Icon (Copy of the real icon)
            _ghostIconObj = Instantiate(_itemToDrag.gameObject, _inventoryUI.mainCanvas.transform);
            _ghostIconObj.GetComponentInChildren<Image>().raycastTarget = false; // Important!

            // Optional: Update Ghost Text to show half amount
            TextMeshProUGUI ghostText = _ghostIconObj.GetComponentInChildren<TextMeshProUGUI>();
            if (ghostText) ghostText.text = _dragAmount.ToString();
        }
        else
        {
            _isSplitDrag = false;
            _dragAmount = slotData.stackSize;

            _originalParent = _itemToDrag.transform.parent;
            _itemToDrag.transform.SetParent(_inventoryUI.mainCanvas.transform);
            _itemIcon.raycastTarget = false;
        }

        if (_inventoryUI.bgButton != null)
            _inventoryUI.bgButton.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _ui.ShowItemToolTip(false);

        if (_isSplitDrag && _ghostIconObj != null)
        {
            _ghostIconObj.transform.position = Input.mousePosition;
        }
        else if (!_isSplitDrag && _itemIcon.enabled)
        {
            _itemToDrag.transform.position = Input.mousePosition;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        _ui.ShowItemToolTip(true);
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Transform playerTransform = _inventoryUI.inventory.transform;
            Vector2 randomOffset = Random.insideUnitCircle * 1.5f;

            // 2. Add it to player position (Cast to Vector3 to match transform)
            Vector3 dropPos = playerTransform.position + (Vector3)randomOffset;

            // 3. Ensure Z axis stays at 0 (or same as player) to prevent sorting issues
            dropPos.z = playerTransform.position.z;

            if (!_isSplitDrag)
                _inventoryUI.inventory.DropItem(_slotIndex, dropPos);

        }

        if (_isSplitDrag)
        {
            // Destroy the ghost copy
            if (_ghostIconObj != null) Destroy(_ghostIconObj);
        }
        else
        {
            _itemToDrag.transform.SetParent(_originalParent);
            _itemToDrag.transform.localPosition = Vector3.zero;
            _itemIcon.raycastTarget = true;
        }


        if (_inventoryUI.bgButton != null)
            _inventoryUI.bgButton.raycastTarget = true;

    }
    public virtual void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObj = eventData.pointerDrag;
        if (droppedObj == null) return;

        UIInventorySlot incomingSlotUI = droppedObj.GetComponent<UIInventorySlot>();
        if (incomingSlotUI != null)
        {
            ItemDataSO incomingItem = incomingSlotUI.GetItem();

            if (CanAccept(incomingItem))
            {
                ItemDataSO myItem = GetItem();

                // Logic:
                // 1. If it's a Split Drag -> Call Transfer
                // 2. If it's a Normal Drag -> Call Swap

                if (incomingSlotUI.IsSplitDrag)
                {
                    // Only transfer if target accepts it or is empty
                    if (myItem == null || incomingSlotUI.CanAccept(myItem))
                    {
                        // Call the NEW function in InventorySystem
                        _inventoryUI.inventory.TransferItem(incomingSlotUI.SlotIndex, this._slotIndex, incomingSlotUI.DragAmount);
                    }
                }
                else
                {
                    // Standard Swap
                    if (myItem == null || incomingSlotUI.CanAccept(myItem))
                    {
                        _inventoryUI.HandleSwap(incomingSlotUI.SlotIndex, this._slotIndex);
                    }
                }
            }
        }
        _isSplitDrag = false;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemData() == null)
            return;

        _ui.itemToolTip.ShowItemToolTip(ItemData());

        _ui.ShowItemToolTip(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        _ui.ShowItemToolTip(false);
    }

    private void OnDisable()
    {
        _ui.ShowItemToolTip(false);

    }
}
