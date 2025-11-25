using NUnit.Framework.Interfaces;
using UnityEngine;

public class UIEquipmentSlot : UIInventorySlot
{
    [Header("Equipment Rules")]
    public EquipmentType allowedType; 

    public override bool CanAccept(ItemDataSO item)
    {
        if (item == null) return true; // Empty is fine

        // Only return true if the types match
        return item.equipmentType == allowedType;
    }
}
