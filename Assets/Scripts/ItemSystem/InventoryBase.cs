using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public InventorySlot[] slots;
    public int maxInventorySize = 24;

    public event Action OnInventoryChanged;

    public bool CanAddItem() => slots.Length < maxInventorySize;
    private void Awake()
    {
        slots = new InventorySlot[maxInventorySize];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new InventorySlot();
        }
    }

    public bool TryAddItem(ItemDataSO itemToAdd, int amountToAdd)
    {
        int difference;
        if(itemToAdd.maxStackSize > 1)
        {
            foreach (InventorySlot slot in slots)
            {
                if(slot.HasItem && slot.itemData == itemToAdd && slot.itemData.maxStackSize >= slot.stackSize + amountToAdd)
                {
                    slot.AddStack(amountToAdd);

                    OnInventoryChanged?.Invoke();
                    return true;
                }
                else if (slot.HasItem && slot.itemData == itemToAdd && slot.itemData.maxStackSize < slot.stackSize + amountToAdd)
                {
                    difference = (slot.stackSize + amountToAdd) - slot.itemData.maxStackSize; ;
                    slot.stackSize = slot.itemData.maxStackSize;
                    amountToAdd = difference;
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if(!slot.HasItem)
            {
                slot.itemData = itemToAdd;
                slot.stackSize = amountToAdd;

                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        return false;

    }

}
