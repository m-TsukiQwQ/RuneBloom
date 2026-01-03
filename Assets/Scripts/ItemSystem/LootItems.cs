using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LootItems
{
    public ItemDataSO item;
    [Range(0f, 100f)] public float dropChance; // 0 to 100%
    public int minAmount = 1;
    public int maxAmount = 1;
}


