using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [Header("Config")]
    public LootTableS0 lootTable;

    [SerializeField] private GameObject _pickUpPrefab;

    public void SpawnLoot()
    {
        if (lootTable == null) return;

        // 1. Calculate drops
        var drops = lootTable.GetLoot();

        // 2. Spawn them
        foreach (var drop in drops)
        {
            SpawnSingleItem(drop.itemData, drop.stackSize);
        }
    }

    private void SpawnSingleItem(ItemDataSO item, int amount)
    {
        if (_pickUpPrefab == null)
        {
            
            return;
        }

        // 1. Calculate Position (Random spread circle)
        Vector2 randomSpread = Random.insideUnitCircle * 0.3f;
        Vector3 spawnPos = transform.position + (Vector3)randomSpread;

        // 2. Instantiate
        GameObject lootObj = Instantiate(_pickUpPrefab, spawnPos, Quaternion.identity);

        // 3. Initialize Data (So 5 Wood contains "5", not "1")
        ObjectItemPickUp pickup = lootObj.GetComponent<ObjectItemPickUp>();
        if (pickup != null)
        {
            pickup.SetObject(item, amount);
        }

        // 4. (Optional) Animation/Bounce
        // If your item has a Rigidbody2D, add a small force
        Rigidbody2D rb = lootObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 popForce = Random.insideUnitCircle.normalized * 3f;
            rb.AddForce(popForce, ForceMode2D.Impulse);
        }
    }
}
