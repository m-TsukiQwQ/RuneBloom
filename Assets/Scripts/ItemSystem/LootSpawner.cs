using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [Header("Config")]
    public LootTableS0 lootTable;

    [SerializeField] private GameObject _pickUpPrefab;

    public void SpawnLoot()
    {
        if (lootTable == null) return;

        // Calculate drops
        var drops = lootTable.GetLoot();

        //Spawn them
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

        // Calculate Position 
        Vector2 randomSpread = Random.insideUnitCircle * 0.3f;
        Vector3 spawnPos = transform.position + (Vector3)randomSpread;

        // Instantiate
        GameObject lootObj = Instantiate(_pickUpPrefab, spawnPos, Quaternion.identity);

        //Initialize Data 
        ObjectItemPickUp pickup = lootObj.GetComponent<ObjectItemPickUp>();
        if (pickup != null)
        {
            pickup.SetObject(item, amount);
        }


        Rigidbody2D rb = lootObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 popForce = Random.insideUnitCircle.normalized * 3f;
            rb.AddForce(popForce, ForceMode2D.Impulse);
        }
    }
}
