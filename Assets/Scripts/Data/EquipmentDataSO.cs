using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Setup/Item Data/Armor item", fileName = "Armor data - ")]
public class EquipmentDataSO : ItemDataSO
{
    [Header("Item modifiers")]
    public ItemModifier[] modifiers;
    
}

[Serializable]
public class ItemModifier
{
    public StatType statType;
    public float value;
    public bool isMultiplier;
}