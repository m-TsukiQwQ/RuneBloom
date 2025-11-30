using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LootDrop
{
    public ItemDataSO item;
    [Range(0f, 100f)] public float dropChance; // 0 to 100%
    public int minAmount = 1;
    public int maxAmount = 1;
}

[CreateAssetMenu(menuName = "Game Setup/Loot Table", fileName = "Loot Table - ")]
public class LootTableSO : ScriptableObject
{
    public List<LootDrop> lootTable;

    public List<InventorySlot> GetLoot()
    {
        List<InventorySlot> drops = new List<InventorySlot>();

        foreach (var entry in lootTable)
        {
            // Roll the dice (0 to 100)
            float roll = Random.Range(0f, 100f);

            if (roll <= entry.dropChance)
            {
                // Calculate random quantity
                int amount = Random.Range(entry.minAmount, entry.maxAmount + 1);

                // Add to list
                drops.Add(new InventorySlot(entry.item, amount));
            }
        }

        return drops;
    }
}
