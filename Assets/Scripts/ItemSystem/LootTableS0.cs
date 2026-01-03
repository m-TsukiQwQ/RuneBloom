using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Setup/Loot Table", fileName = "Loot Table - ")]
public class LootTableS0 : ScriptableObject
{
    public List<LootItems> lootTable;

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
