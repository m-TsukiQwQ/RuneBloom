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


    [SerializeField] private UIInventory _inventoryUI;
    [SerializeField] private Transform _originalParent; // To remember where the icon belongs
     [SerializeField] private Canvas _mainCanvas;


    [SerializeField] private UIManager _ui;

    private InventoryBase _myInventory;
    public InventoryBase OwnerInventory => _myInventory;

    private int _slotIndex;
    // Public getter so the Drop Target knows if we are splitting
    public bool IsSplitDrag => _isSplitDrag;
    public int DragAmount => _dragAmount;

    private ItemDataSO ItemData()
    {
        return _myInventory.slots[SlotIndex].itemData;
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
        _inventoryUI = _ui.GetComponentInChildren<UIInventory>(true);
    }
    public void Init(int index, InventoryBase inventory, Canvas canvas)
    {
        _slotIndex = index;
        _myInventory = inventory;
        _mainCanvas = canvas;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _ui.ShowItemToolTip(false);

        if (_itemIcon.enabled == false) return;
        InventorySlot slotData = _myInventory.slots[_slotIndex];

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

        if (_inventoryUI.darkBGButton != null)
            _inventoryUI.darkBGButton.raycastTarget = false;
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

            Vector3 dropPos = playerTransform.position + (Vector3)randomOffset;
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


        if (_inventoryUI.darkBGButton != null)
            _inventoryUI.darkBGButton.raycastTarget = true;

    }
    public virtual void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObj = eventData.pointerDrag;
        if (droppedObj == null) return;

        UIInventorySlot incomingSlotUI = droppedObj.GetComponent<UIInventorySlot>();
        if (incomingSlotUI != null)
        {
            ItemDataSO incomingItem = incomingSlotUI.GetItem();

            //if (CanAccept(incomingItem))
            //{
            //    ItemDataSO myItem = GetItem();

            //    if (incomingSlotUI.OwnerInventory == this.OwnerInventory)
            //        // Logic:
            //        // 1. If it's a Split Drag -> Call Transfer
            //        // 2. If it's a Normal Drag -> Call Swap


            //        if (incomingSlotUI.IsSplitDrag)
            //    {
            //        // Only transfer if target accepts it or is empty
            //        if (myItem == null || incomingSlotUI.CanAccept(myItem))
            //        {
            //            // Call the NEW function in InventorySystem
            //            _inventoryUI.inventory.TransferItem(incomingSlotUI.SlotIndex, this._slotIndex, incomingSlotUI.DragAmount);
            //        }
            //    }
            //    else
            //    {
            //        // Standard Swap
            //        if (myItem == null || incomingSlotUI.CanAccept(myItem))
            //        {
            //            if (incomingSlotUI.OwnerInventory == this.OwnerInventory)
            //            {
            //                // Standard Swap
            //                _myInventory.SwapItems(incomingSlotUI.SlotIndex, this._slotIndex);
            //            }
            //            // CASE 2: Different Owner (e.g. Bag to Chest)
            //            else
            //            {
            //                // Transfer from THEM -> TO ME
            //                incomingSlotUI.OwnerInventory.TransferTo(this.OwnerInventory, incomingSlotUI.SlotIndex, this._slotIndex);
            //            }
            //            //_inventoryUI.HandleSwap(incomingSlotUI.SlotIndex, this._slotIndex);
            //        }
            //    }
            //}

            if (CanAccept(incomingItem))
            {
                ItemDataSO myItem = GetItem();

                // Check 1: Is this a LOCAL operation? (Same Inventory)
                if (incomingSlotUI.OwnerInventory == this.OwnerInventory)
                {
                    if (myItem == null || incomingSlotUI.CanAccept(myItem))
                    {
                        // Split Drag (Right Click) -> Local Transfer
                        if (incomingSlotUI.IsSplitDrag)
                        {
                            // Note: You need to ensure TransferItem exists in InventoryBase or cast to InventorySystem
                            // Assuming you moved TransferItem to Base as discussed:
                            _myInventory.TransferItem(incomingSlotUI.SlotIndex, this._slotIndex, incomingSlotUI.DragAmount);
                        }
                        // Normal Drag (Left Click) -> Local Swap
                        else
                        {
                            _myInventory.SwapItems(incomingSlotUI.SlotIndex, this._slotIndex);
                        }
                    }
                }
                // Check 2: Different Owner (Chest <-> Player)
                else
                {
                    // Call the Cross-Inventory Transfer logic
                    // Logic: Take from THEM, Give to ME

                    // Note: If splitting across inventories, we ideally pass the DragAmount.
                    // If TransferTo doesn't support amount yet, we default to full stack or you need to update InventoryBase.
                    // Ideally: incomingSlotUI.OwnerInventory.TransferTo(this.OwnerInventory, incomingSlotUI.SlotIndex, this._slotIndex, incomingSlotUI.DragAmount);

                    // For now, using standard TransferTo:
                    incomingSlotUI.OwnerInventory.TransferTo(this.OwnerInventory, incomingSlotUI.SlotIndex, this._slotIndex);
                }
            }
        }
        _isSplitDrag = false;
    }
    

    public int SlotIndex => _slotIndex;

    public ItemDataSO GetItem()
    {
        if (_slotIndex < _myInventory.slots.Length)
            return _myInventory.slots[_slotIndex].itemData;
        return null;
    }

    public virtual bool CanAccept(ItemDataSO item)
    {
        return true; // Normal slots accept everything
    }

    public virtual void UpdateSlot(InventorySlot slot)
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
                if (_myInventory is PlayerInventory playerSystem)
                {
                    // We need to access the UI Manager to run logic, or call logic on the system
                    // If OnQuickEquip is on UIInventory (Player UI), we can call it:
                    if (_inventoryUI != null) _inventoryUI.OnQuickEquip(_slotIndex, 1);
                }
                else
                {
                    // Logic: Loot entire stack to Player
                    // We need to find the player's inventory to send it there.
                    // Usually we find the Player Singleton or reference.

                    PlayerInventory player = FindFirstObjectByType<PlayerInventory>();
                    if (player != null)
                    {
                        // Transfer from ME (Chest) -> PLAYER
                        // -1 as target index implies "Find First Empty Slot"
                        _myInventory.TransferTo(player, _slotIndex, -1);
                    }
                }
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
