using UnityEngine;

public class PlayerInventory : InventoryBase
{

    private EntityStats _playerStats;

    [SerializeField] private int _equipmentStartIndex = 32;
    public int inventoryStartIndex => _inventoryStartIndex;
    [SerializeField] private int _inventoryStartIndex = 8;



    protected override void Awake()
    {
        _playerStats = GetComponent<EntityStats>();
        base.Awake();
    }

    private void HandleEquipmentModifier()
    {
        if (_playerStats == null) return;
        for (int i = _equipmentStartIndex; i < slots.Length; i++)
        {
            InventorySlot slot = slots[i];
            slot.ManageModifier(_playerStats);
        }
    }

    public override bool TryAddItem(ItemDataSO itemToAdd, int amountToAdd)
    {

        bool result = base.TryAddItem(itemToAdd, amountToAdd);
        HandleEquipmentModifier();
        return result;



    }

    public override void DropItem(int slotIndex, Vector2 spawnPoint)
    {
        base.DropItem(slotIndex, spawnPoint);
        HandleEquipmentModifier();
    }

    public override void SwapItems(int indexA, int indexB)
    {
        InventorySlot slotA = slots[indexA];
        InventorySlot slotB = slots[indexB];

        if (!slotA.HasItem) return;
        // If we move a Helmet from Equip(A) to Bag(B), we must remove its stats first.
        // We do this for BOTH slots to be safe.
        if (slotA != null && _playerStats != null) slotA.RemoveModifiers(_playerStats);
        if (slotB != null && _playerStats != null) slotB.RemoveModifiers(_playerStats);

        base.SwapItems(indexA, indexB);

        HandleEquipmentModifier();
    }

    public override void TransferItem(int fromIndex, int toIndex, int amountToMove)
    {
        base.TransferItem(fromIndex, toIndex, amountToMove);
        HandleEquipmentModifier();
    }

    public override void TransferTo(InventoryBase targetInventory, int sourceIndex, int targetIndex)
    {
        // If we are moving an item OUT of the player, check if it had stats applied
        InventorySlot mySlot = slots[sourceIndex];
        if (mySlot != null && _playerStats != null)
        {
            mySlot.RemoveModifiers(_playerStats);
        }

        // Run the standard transfer logic
        base.TransferTo(targetInventory, sourceIndex, targetIndex);

        // Ensure stats are up to date
        HandleEquipmentModifier();
    }

    public void SortPlayersInventory()
    {
        SortInventory(_inventoryStartIndex, _equipmentStartIndex);
    }
}
