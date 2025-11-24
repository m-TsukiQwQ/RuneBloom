using Unity.VisualScripting;
using UnityEngine;

public class ObjectItemPickUp : MonoBehaviour
{
    private SpriteRenderer _sr;
    [SerializeField]
    private ItemDataSO _itemData;
    [SerializeField]
    private int _amount = 1;

    private InventorySlot _itemToAdd;
    private Inventory_Base _inventory;


    private void OnValidate()
    {
        if (_itemData == null) return;

        _sr = GetComponentInChildren<SpriteRenderer>();
        if (_sr != null)
            _sr.sprite = _itemData.itemIcon;
        gameObject.name = "Object_ItemPickup - " + _itemData.itemName;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Try to find the InventorySystem on the player
        _inventory = collision.GetComponent<Inventory_Base>();


        if (_inventory != null)
        {
            // 2. Call our AddItem function
            // It returns 'true' if successful, 'false' if full
            bool wasAdded = _inventory.TryAddItem(_itemData, _amount);

            if (wasAdded)
            {
                Destroy(gameObject);
            }
        }
    }

}
