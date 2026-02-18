using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBase : MonoBehaviour
{
    public InventorySlot[] slots;
    public int maxInventorySize;

    public InventoryBase CurrentOpenChest { get; set; }


    public event Action OnInventoryChanged;

    [SerializeField] protected GameObject _pickupPrefab;

    protected virtual void Awake()
    {
        
        slots = new InventorySlot[maxInventorySize];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new InventorySlot();
        }
    }

    public virtual void RemoveItem(int slotIndex, int amountToRemove)
    {
        if (slots[slotIndex].stackSize <= 0)
            slots[slotIndex].Clear();
        OnInventoryChanged?.Invoke();
    }



    public virtual bool TryAddItem(ItemDataSO itemToAdd, int amountToAdd)
    {
        // 1. Try to stack into existing slots first
        if (itemToAdd.maxStackSize > 1)
        {
            foreach (InventorySlot slot in slots)
            {
                // Is this the right item and is there space?
                if (slot.HasItem && slot.itemData == itemToAdd && slot.stackSize < itemToAdd.maxStackSize)
                {
                    int spaceRemaining = itemToAdd.maxStackSize - slot.stackSize;
                    
                    // Take whichever is smaller: the space we have, or the amount we need to add
                    int amountToStack = Mathf.Min(spaceRemaining, amountToAdd);
                    
                    slot.AddToStack(amountToStack);
                    amountToAdd -= amountToStack; // Reduce the amount we still need to place
                    
                    OnInventoryChanged?.Invoke();

                    // If we have placed everything, we are done!
                    if (amountToAdd <= 0) return true;
                }
            }
        }

        // 2. Fill empty slots with whatever is remaining
        foreach (InventorySlot slot in slots)
        {
            if (!slot.HasItem)
            {
                int amountToAssign = Mathf.Min(amountToAdd, itemToAdd.maxStackSize);
                
                slot.AssignItem(itemToAdd, amountToAssign);
                amountToAdd -= amountToAssign;
                
                OnInventoryChanged?.Invoke();

                // If we have placed everything, we are done
                if (amountToAdd <= 0) return true;
            }
        }

        // If we exit the loops and amountToAdd is still > 0, it means we ran out of space.
        // Usually, you might drop the remainder on the ground here, or just return false.
        return amountToAdd <= 0;
    }

    protected virtual void Start()
    {
        OnInventoryChanged?.Invoke();
    }




    public virtual void DropItem(int slotIndex, Vector2 spawnPoint)
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
        
    }
    public virtual void SwapItems(int indexA, int indexB)
    {
        InventorySlot slotA = slots[indexA];
        InventorySlot slotB = slots[indexB];

        if (!slotA.HasItem) return;



        if (slotB.itemData == slotA.itemData) // Stacking onto the same item
        {
            if (slotB.stackSize + slotA.stackSize <= slotB.itemData.maxStackSize)
            {
                slotB.AddToStack(slotA.stackSize);
                slotA.stackSize -= slotA.stackSize;
                slotA.Clear();
            }
            else
            {
                //Handle overflow 
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
        
    }

    public virtual void TransferItem(int fromIndex, int toIndex, int amountToMove)
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
                //Handle overflow
                int space = toSlot.itemData.maxStackSize - toSlot.stackSize;
                toSlot.AddToStack(space);
                fromSlot.stackSize -= space;
            }
        }
       
        if (fromSlot.stackSize <= 0)
        {
            fromSlot.Clear();
        }

        OnInventoryChanged?.Invoke();

    }

    public void SortInventory(int startOfList, int endOfList)
    {
        List<InventorySlot> listOfItems = new List<InventorySlot>();

        for (int i = startOfList; i < endOfList; i++)
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


        int index = startOfList;
        int listIndex = 0;
        foreach (var item in compactedList)
        {
            slots[index].AssignItem(compactedList[listIndex].itemData, compactedList[listIndex].stackSize);

            index++;
            listIndex++;
        }


        OnInventoryChanged?.Invoke();


    }


    // Moves an item from THIS inventory -> TARGET inventory
    public virtual void TransferTo(InventoryBase targetInventory, int sourceIndex, int targetIndex)
    {
        InventorySlot mySlot = slots[sourceIndex];
        InventorySlot targetSlot = targetInventory.slots[targetIndex];

        // Safety Checks
        if (!mySlot.HasItem) return;

        // Logic A: Target is Empty -> Move everything
        if (!!targetSlot.HasItem)
        {
            targetSlot.AssignItem(mySlot.itemData, mySlot.stackSize);
            mySlot.Clear();
        }
        // Logic B: Target matches Item -> Stack it
        else if (targetSlot.itemData == mySlot.itemData)
        {
            // Simple stack logic (assuming infinite for now or standard max)
            int space = targetSlot.itemData.maxStackSize - targetSlot.stackSize;
            int amountToMove = Mathf.Min(space, mySlot.stackSize);

            if (amountToMove > 0)
            {
                targetSlot.AddToStack(amountToMove);
                mySlot.stackSize -= amountToMove;
                if (mySlot.stackSize <= 0) mySlot.Clear();
            }
            else
            {
                // No space to stack, do nothing (or swap if we implemented logic for that)
                return;
            }
        }
        else
        {
            // 1. Capture Target Data (e.g. Apple in Chest)
            ItemDataSO tempItem = targetSlot.itemData;
            int tempStack = targetSlot.stackSize;

            // 2. Move Source -> Target (Sword to Chest)
            targetSlot.AssignItem(mySlot.itemData, mySlot.stackSize);

            // 3. Move Temp -> Source (Apple to Player)
            mySlot.AssignItem(tempItem, tempStack);
        }

        //Both inventories changed, so both UIs must redraw
        this.OnInventoryChanged?.Invoke();
        targetInventory.OnInventoryChanged?.Invoke();
    }


    public int GetTotalAmount(ItemDataSO itemData)
    {
        int totalAmount = 0;
        foreach (var slot in slots)
        {
            if (slot.itemData == itemData && slot.HasItem)
                totalAmount += slot.stackSize;
        }

        return totalAmount;
    }

    public bool RemoveItem(ItemDataSO itemData, int amountToRemove)
    {
        if (itemData == null) return false;

        if (GetTotalAmount(itemData) < amountToRemove) return false;

        foreach (var slot in slots)
        {
            if(slot.HasItem && slot.itemData == itemData)
            {
                if(slot.stackSize > amountToRemove)
                {
                    slot.stackSize -= amountToRemove;
                    amountToRemove = 0;
                }
                else
                {
                    amountToRemove -= slot.stackSize;
                    slot.Clear();
                }
                if (amountToRemove <= 0) break;
            }
        }

        OnInventoryChanged?.Invoke();
        return true;
    }
}
