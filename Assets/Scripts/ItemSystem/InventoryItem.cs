using System;
using UnityEngine;

[Serializable]
public class Inventory_Item
{
    public ItemDataSO itemData;
    public int stackSize = 1;

    //public ItemModifier[] modifiers {get; private set;}

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        //modifiers = EqupmentData()?.modifiers;
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

    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
