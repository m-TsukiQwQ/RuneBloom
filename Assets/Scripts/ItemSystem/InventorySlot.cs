using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemDataSO itemData;
    public int stackSize = 1;

    //public ItemModifier[] modifiers {get; private set;}

    public InventorySlot(ItemDataSO itemData, int amount)
    {
        this.itemData = itemData;
        stackSize = amount;
        //modifiers = EqupmentData()?.modifiers;
    }

    public InventorySlot() // Empty Constructor
    {
        Clear();
    }

    public void Clear()
    {
        itemData = null;
        stackSize = 0;
    }

    //public void AddModifiers(EntityState playerStats)
    //{
    //    foreach(var modifier in modifiers)
    //}
    //private EquipmentDataSO EquipmentData()
    //{
    //    if (itemData is EquipmentDataSO eqiupment)
    //        return eqiupment;

    //    return null;
    //}

    public bool HasItem => itemData != null;
    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    public void AddStack(int amount) => stackSize += amount ;
    public void RemoveStack() => stackSize--;
}
