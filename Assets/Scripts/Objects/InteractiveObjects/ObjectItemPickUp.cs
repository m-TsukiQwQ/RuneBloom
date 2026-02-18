using System.Collections;
using UnityEngine;

public class ObjectItemPickUp : MonoBehaviour
{
    private SpriteRenderer _sr;
    [SerializeField] private ItemDataSO _itemData;
    [SerializeField] private int _amount = 1;

    private InventoryBase _inventory;

    [Header("Settings")]
    public float pickupDelay = 0.5f; // Prevent picking up instantly after dropping
    private bool _canPickup = false;
    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        SetObject(_itemData, _amount);
    }

    private void Start()
    {
        StartCoroutine(EnablePickupRoutine());
    }

    private IEnumerator EnablePickupRoutine()
    {
        yield return new WaitForSeconds(pickupDelay);
        _canPickup = true;
    }

    public void SetObject(ItemDataSO itemData, int amount)
    {
        if (_itemData == null) return;
        if (_sr != null)
            _sr.sprite = itemData.itemIcon;
        if (_itemData is BuildableItemSO buidable)
        {
            if (buidable.GameObject == null)
            {
                _sr.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
            _amount = amount;
            _itemData = itemData;

        
    }

    private void OnValidate()
    {
        if (_itemData == null) return;

        _sr = GetComponentInChildren<SpriteRenderer>();
        if (_sr != null)
            _sr.sprite = _itemData.itemIcon;
        if (_itemData is BuildableItemSO buidable)
        {
            if (buidable.GameObject == null)
            {
                _sr.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
        gameObject.name = "Object_ItemPickup - " + _itemData.itemName;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_canPickup)
            return;
        //Try to find the InventorySystem on the player
        _inventory = collision.GetComponent<PlayerInventory>();


        if (_inventory != null)
        {
            // Call our AddItem function
            // It returns 'true' if successful, 'false' if full
            bool wasAdded = _inventory.TryAddItem(_itemData, _amount);

            if (wasAdded)
            {
                Destroy(gameObject);
            }
        }
    }

}
