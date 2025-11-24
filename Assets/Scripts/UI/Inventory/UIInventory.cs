using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [SerializeField] UIInventorySlot _slotPrefab;
    private UIInventorySlot[] _uiItemSlots;

    [Header("References")]
    private Inventory_Base _inventory;
    public Transform itemsParent;
    public Canvas mainCanvas;


    private void Awake()
    {

        _uiItemSlots = itemsParent.GetComponentsInChildren<UIInventorySlot>();
        if(_inventory == null)
            _inventory = FindFirstObjectByType<Inventory_Base>();
        if(mainCanvas == null)
            mainCanvas = GetComponentInParent<Canvas>();

        InitializeInventory();

        _inventory.OnInventoryChanged += UpdateUI;

        for (int i = 0; i < _uiItemSlots.Length; i++)
        {
            // Pass the Canvas to the slot so it can reparent the icon
            _uiItemSlots[i].Init(i, mainCanvas);
        }

        UpdateUI();
    }

    private void InitializeInventory()
    {
        // 1. Create the array with the correct, known size.
        _uiItemSlots = new UIInventorySlot[_inventory.maxInventorySize];

        for (int i = 0; i < _inventory.maxInventorySize; i++)
        {
            // 2. Create the slot from the prefab.
            UIInventorySlot slot = Instantiate(_slotPrefab);

            // 3. Parent the new slot to this GameObject (the UI Panel).
            //    Set 'worldPositionStays' to false to prevent layout issues.
            slot.transform.SetParent(itemsParent, false);

            // 4. Add the newly created slot to our array.
            _uiItemSlots[i] = slot;

            // 5. (Optional but recommended) Initialize the slot as empty.
            //    I'm assuming your UpdateSlot(null) method handles this.
            slot.UpdateSlot(null);
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < _inventory.slots.Length; i++)
        {
            if(i < _uiItemSlots.Length)
            {
                _uiItemSlots[i].UpdateSlot(_inventory.slots[i]);
            }
        }
        
    }

    
}
