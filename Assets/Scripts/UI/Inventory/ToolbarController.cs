using InputSystem;
using UnityEngine;

public class ToolbarController : MonoBehaviour
{

    private EntityStats _stats;
    private PlayerInventory _inventory;
    private PlayerHunger _hunger;
    private PlayerHealth _health;

    [SerializeField] private SpriteRenderer _itemInHand;
    [SerializeField] private BuildingPlacer _buildingPlacer;

    private int _toolbarSize;

    private int _selectedSlot = -1;


    [field: SerializeField] public BuildableItemSO activeBuildable { get; private set; }

    [SerializeField] private ConstructionLayer _constructionLayer;
    [SerializeField] private MouseUser _mouseUser;

    [SerializeField] private PreviewLayer _previewLayer;


    private void Awake()
    {
        _stats = GetComponentInParent<EntityStats>();
        _inventory = GetComponentInParent<PlayerInventory>();
        _hunger = GetComponentInParent<PlayerHunger>();
        _health = GetComponentInParent<PlayerHealth>();

        _toolbarSize = _inventory.inventoryStartIndex;
        _itemInHand.sprite = null;

    }

    private void Update()
    {
        HandleScroll();
    }

    private void Start()
    {
        if (_buildingPlacer != null)
            _buildingPlacer.OnPlaceSuccess += HandleBuildingPlaced;
        _selectedSlot = 0;
    }
    private void OnDestroy()
    {
        if (_buildingPlacer != null)
            _buildingPlacer.OnPlaceSuccess -= HandleBuildingPlaced;
    }

    private void ShowSelectedItem()
    {
        ItemDataSO itemToHold = _inventory.slots[_selectedSlot].itemData;
        if (itemToHold == null || itemToHold.inHandIcon == null)
        {
            _itemInHand.sprite = null;
            return;
        }

        _itemInHand.sprite = itemToHold.inHandIcon;
    }

    public ItemDataSO GetSelectedItem()
    {
        ItemDataSO item = _inventory.slots[_selectedSlot].itemData;
        return item;
    }

    private void HandleBuildingPlaced()
    {
        // Remove 1 item
        _inventory.RemoveItem(_selectedSlot, 1);

        // Check if we ran out of items
        if (_inventory.slots[_selectedSlot].stackSize <= 0)
        {
            // Clear the hand and the builder
            _buildingPlacer.ChangeBuildable(null);
            _itemInHand.sprite = null;
        }
    }


    private void HandleScroll()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 9)
            {
                ChangeSelectedSlot(number - 1);
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            int newValue = _selectedSlot - (int)(scroll / Mathf.Abs(scroll));
            if (newValue < 0)
            {
                newValue = _toolbarSize - 1;
            }
            else if (newValue >= _toolbarSize)
            {
                newValue = 0;
            }
            ChangeSelectedSlot(newValue);
        }
        ShowSelectedItem();
    }

    public void ChangeSelectedSlot(int newValue)
    {
        InventorySlot oldSlot = _inventory.slots[_selectedSlot];
        if (oldSlot != null)
        {
            oldSlot.SimpleRemoveModifiers(_stats, "Selected item");
        }


        _selectedSlot = newValue;
        InventorySlot newSlot = _inventory.slots[_selectedSlot];
        newSlot.SimpleAddModifier(_stats, "Selected item");
        if(newSlot.itemData is BuildableItemSO buildable)
        {

            _buildingPlacer.ChangeBuildable(buildable);
        }
        else
            _buildingPlacer.ChangeBuildable(null);
    }

    public void UseItem()
    {
        var selectedSlotItem = _inventory.slots[_selectedSlot].itemData;
        if(selectedSlotItem is FoodDataSO food)
        {
            if (food.hunger != 0)
                _hunger.RestoreHunger(food.hunger);
            if (food.health != 0)
                _health.IncreaseHealth(food.health);

            _inventory.RemoveItem(_selectedSlot, 1);
        }
        
        
    }
}

