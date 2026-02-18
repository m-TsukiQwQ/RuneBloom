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

            float roll = Random.Range(0f, 100f);

            if (roll <= entry.dropChance)
            {

                int amount = Random.Range(entry.minAmount, entry.maxAmount + 1);


                drops.Add(new InventorySlot(entry.item, amount));
            }
        }

        return drops;
    }
}
