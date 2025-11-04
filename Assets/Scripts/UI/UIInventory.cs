using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    private UIItemSlot[] _uiItemSlots;
    private Inventory_Base _inventory;
    [SerializeField] UIItemSlot _slotPrefab;

    private void Awake()
    {
        _uiItemSlots = GetComponentsInChildren<UIItemSlot>();
        _inventory = FindFirstObjectByType<Inventory_Base>();

        InitializeInventory();
        _inventory.OnInventoryChange += UpdateInventorySlots;
        UpdateInventorySlots();
    }

    private void InitializeInventory()
    {
        // 1. Create the array with the correct, known size.
        _uiItemSlots = new UIItemSlot[_inventory.maxInventorySize];

        for (int i = 0; i < _inventory.maxInventorySize; i++)
        {
            // 2. Create the slot from the prefab.
            UIItemSlot slot = Instantiate(_slotPrefab);

            // 3. Parent the new slot to this GameObject (the UI Panel).
            //    Set 'worldPositionStays' to false to prevent layout issues.
            slot.transform.SetParent(this.transform, false);

            // 4. Add the newly created slot to our array.
            _uiItemSlots[i] = slot;

            // 5. (Optional but recommended) Initialize the slot as empty.
            //    I'm assuming your UpdateSlot(null) method handles this.
            slot.UpdateSlot(null);
        }
    }

    private void UpdateInventorySlots()
    {
        List<Inventory_Item> itemList = _inventory.itemList;
        for (int i = 0; i < _uiItemSlots.Length; i++)
        {
            if (i < itemList.Count)
            {
                _uiItemSlots[i].UpdateSlot(itemList[i]);
            }
            else
            {
                _uiItemSlots[i].UpdateSlot(null);
            }
        }
    }
}
