using UnityEngine;

public class ObjectItemPickUp : MonoBehaviour
{
    private SpriteRenderer _sr;
    [SerializeField]
    private ItemDataSO _itemData;

    private Inventory_Item _itemToAdd;
    private Inventory_Base _inventory;

    private void Awake()
    {
        _itemToAdd = new Inventory_Item(_itemData);
    }

    private void OnValidate()
    {
        if (_itemData == null) return;

        _sr = GetComponentInChildren<SpriteRenderer>();
        _sr.sprite = _itemData.itemIcon;
        gameObject.name = "Object_ItemPickup - " + _itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _inventory = collision.GetComponent<Inventory_Base>();
        if (_inventory != null && _inventory.CanAddItem())
        {
            _inventory.AddItem(_itemToAdd);
            Destroy(gameObject);
        }
    }

}
