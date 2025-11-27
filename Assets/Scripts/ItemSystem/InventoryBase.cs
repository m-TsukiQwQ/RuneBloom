using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBase : MonoBehaviour
{
    [SerializeField] private EntityStats _playerStats;

    public InventorySlot[] slots;
    public int maxInventorySize;
    [SerializeField] private int _equipmentStartIndex = 32;
    [SerializeField] private int _inventoryStartIndex = 8;

    public event Action OnInventoryChanged;

    [SerializeField] private GameObject _pickupPrefab;

    private void Awake()
    {
        _playerStats = GetComponent<EntityStats>();
        slots = new InventorySlot[maxInventorySize];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new InventorySlot();
        }
    }



    public bool TryAddItem(ItemDataSO itemToAdd, int amountToAdd)
    {
        int difference;
        if (itemToAdd.maxStackSize > 1)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.HasItem && slot.itemData == itemToAdd)
                {
                    if (slot.itemData.maxStackSize >= slot.stackSize + amountToAdd)
                    {
                        slot.AddToStack(amountToAdd);

                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                    else if (slot.itemData.maxStackSize < slot.stackSize + amountToAdd)
                    {
                        difference = (slot.stackSize + amountToAdd) - slot.itemData.maxStackSize; ;
                        slot.stackSize = slot.itemData.maxStackSize;
                        amountToAdd = difference;

                        OnInventoryChanged?.Invoke();

                    }
                }

            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (!slot.HasItem)
            {
                slot.AssignItem(itemToAdd, amountToAdd);

                OnInventoryChanged?.Invoke();
                HandleEquipmentModifier();
                return true;
            }
        }

        return false;

    }




    private void HandleEquipmentModifier()
    {
        if (_playerStats == null) return;
        for (int i = _equipmentStartIndex; i < slots.Length; i++)
        {
            InventorySlot slot = slots[i];
            slot.ManageModifier(_playerStats);
        }
    }

    public void DropItem(int slotIndex, Vector2 spawnPoint)
    {
        InventorySlot slot = slots[slotIndex];
        if (!slot.HasItem) return;

        if (_pickupPrefab != null)
        {
            GameObject item = Instantiate(_pickupPrefab, spawnPoint, Quaternion.identity);
            ObjectItemPickUp _pickup = item.GetComponent<ObjectItemPickUp>();
            _pickup.SetObject(slots[slotIndex].itemData, slots[slotIndex].stackSize);
        }
        slot.Clear();

        OnInventoryChanged?.Invoke();
        HandleEquipmentModifier();
    }
    public void SwapItems(int indexA, int indexB)
    {
        InventorySlot slotA = slots[indexA];
        InventorySlot slotB = slots[indexB];

        if (!slotA.HasItem) return;
        // If we move a Helmet from Equip(A) to Bag(B), we must remove its stats first.
        // We do this for BOTH slots to be safe.
        if (slotA != null && _playerStats != null) slotA.RemoveModifiers(_playerStats);
        if (slotB != null && _playerStats != null) slotB.RemoveModifiers(_playerStats);


        if (slotB.itemData == slotA.itemData) // CASE 2: Stacking onto the same item
        {
            if (slotB.stackSize + slotA.stackSize <= slotB.itemData.maxStackSize)
            {
                slotB.AddToStack(slotA.stackSize);
                slotA.stackSize -= slotA.stackSize;
                slotA.Clear();
            }
            else
            {
                // Optional: Handle overflow (fill target, keep rest in source)
                int space = slotB.itemData.maxStackSize - slotB.stackSize;
                slotB.AddToStack(space);
                slotA.stackSize -= space;

            }
        }
        else
        {
            InventorySlot temp = slots[indexA];
            slots[indexA] = slots[indexB];
            slots[indexB] = temp;

        }


        OnInventoryChanged?.Invoke();
        HandleEquipmentModifier();
    }

    public void TransferItem(int fromIndex, int toIndex, int amountToMove)
    {
        InventorySlot fromSlot = slots[fromIndex];
        InventorySlot toSlot = slots[toIndex];
        

        if (!fromSlot.HasItem || amountToMove > fromSlot.stackSize) return;

        // CASE 1: Moving to an Empty Slot
        if (!toSlot.HasItem)
        {
            toSlot.AssignItem(fromSlot.itemData, amountToMove);
            fromSlot.stackSize -= amountToMove;
        }
        else if (toSlot.itemData == fromSlot.itemData) // CASE 2: Stacking onto the same item
        {
            if (toSlot.stackSize + amountToMove <= toSlot.itemData.maxStackSize)
            {
                toSlot.AddToStack(amountToMove);
                fromSlot.stackSize -= amountToMove;
                fromSlot.Clear();
            }
            else
            {
                // Optional: Handle overflow (fill target, keep rest in source)
                int space = toSlot.itemData.maxStackSize - toSlot.stackSize;
                toSlot.AddToStack(space);
                fromSlot.stackSize -= space;
            }
        }
        // Cleanup: If source is now empty, clear it
        if (fromSlot.stackSize <= 0)
        {
            fromSlot.Clear();
        }

        OnInventoryChanged?.Invoke();
        HandleEquipmentModifier();
    }

    public void SortInventory()
    {
        List<InventorySlot> listOfItems = new List<InventorySlot>();

        for (int i = _inventoryStartIndex; i < _equipmentStartIndex; i++)
        {
            InventorySlot slot = slots[i];
            if (slot.HasItem)
            {
                listOfItems.Add(new InventorySlot(slot.itemData, slot.stackSize));
                slot.Clear();
            }

        }

        List<InventorySlot> compactedList = new List<InventorySlot>();

        foreach (var sourceItem in listOfItems)
        {
            // If item is not stackable, just add it directly
            if (sourceItem.itemData.maxStackSize <= 1)
            {
                compactedList.Add(sourceItem);
                continue;
            }

            bool fullyAdded = false;

            // Try to add this source item to existing stacks in our new list
            foreach (var targetItem in compactedList)
            {
                // Match found AND space available?
                if (targetItem.itemData == sourceItem.itemData && targetItem.stackSize < targetItem.itemData.maxStackSize)
                {
                    int space = targetItem.itemData.maxStackSize - targetItem.stackSize;
                    int amountToAdd = Mathf.Min(space, sourceItem.stackSize);

                    targetItem.stackSize += amountToAdd;
                    sourceItem.stackSize -= amountToAdd;

                    if (sourceItem.stackSize == 0)
                    {
                        fullyAdded = true;
                        break;
                    }
                }
            }

            // If we couldn't fit everything into existing stacks, make a new one
            if (!fullyAdded && sourceItem.stackSize > 0)
            {
                compactedList.Add(sourceItem);
            }
        }

        compactedList.Sort((a, b) => string.Compare(a.itemData.itemName, b.itemData.itemName));


        //for (int i = _inventoryStartIndex; i < _equipmentStartIndex; i++)
        //{
        //    slots[i].AssignItem(listOfItems[i].itemData, listOfItems[i].stackSize);


        //}

        int index = _inventoryStartIndex;
        int listIndex = 0;
        foreach (var item in compactedList)
        {
            slots[index].AssignItem(compactedList[listIndex].itemData, compactedList[listIndex].stackSize);

            index++;
            listIndex++;
        }


        OnInventoryChanged?.Invoke();


    }



}
