using System;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    [SerializeField] private EntityStats _playerStats;

    public InventorySlot[] slots;
    public int maxInventorySize;
    [SerializeField] private int equipmentStartIndex = 32;

    public event Action OnInventoryChanged;

    [SerializeField] private GameObject _pickupPrefab;

    private void Awake()
    {
        _playerStats = GetComponent<EntityStats>();
        slots = new InventorySlot[maxInventorySize];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new InventorySlot();
        }
    }

    public bool TryAddItem(ItemDataSO itemToAdd, int amountToAdd)
    {
        int difference;
        if (itemToAdd.maxStackSize > 1)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.HasItem && slot.itemData == itemToAdd)
                {
                    if (slot.itemData.maxStackSize >= slot.stackSize + amountToAdd)
                    {
                        slot.AddStack(amountToAdd);

                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                    else if (slot.itemData.maxStackSize < slot.stackSize + amountToAdd)
                    {
                        difference = (slot.stackSize + amountToAdd) - slot.itemData.maxStackSize; ;
                        slot.stackSize = slot.itemData.maxStackSize;
                        amountToAdd = difference;

                        OnInventoryChanged?.Invoke();

                    }
                }

            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (!slot.HasItem)
            {
                slot.AssignItem(itemToAdd, amountToAdd);

                OnInventoryChanged?.Invoke();
                HandleEquipmentModifier();
                return true;
            }
        }

        return false;

    }



    public void SwapItems(int indexA, int indexB)
    {
        InventorySlot slotA = slots[indexA];
        InventorySlot slotB = slots[indexB];

        // If we move a Helmet from Equip(A) to Bag(B), we must remove its stats first.
        // We do this for BOTH slots to be safe.
        if (slotA != null && _playerStats != null) slotA.RemoveModifiers(_playerStats);
        if (slotB != null && _playerStats != null) slotB.RemoveModifiers(_playerStats);


        InventorySlot temp = slots[indexA];
        slots[indexA] = slots[indexB];
        slots[indexB] = temp;

        OnInventoryChanged?.Invoke();
        HandleEquipmentModifier();
    }

    private void HandleEquipmentModifier()
    {
        if (_playerStats == null) return;
        for (int i = equipmentStartIndex; i < slots.Length; i++)
        {
            InventorySlot slot = slots[i];
            slot.ManageModifier(_playerStats);
        }
    }

    public void DropItem(int slotIndex, Vector2 spawnPoint)
    {
        InventorySlot slot = slots[slotIndex];
        if (!slot.HasItem) return;

        if(_pickupPrefab != null)
        {
            GameObject item = Instantiate(_pickupPrefab, spawnPoint, Quaternion.identity);
            ObjectItemPickUp _pickup = item.GetComponent<ObjectItemPickUp>();
            _pickup.SetObject(slots[slotIndex].itemData, slots[slotIndex].stackSize);
        }
        slot.Clear();

        OnInventoryChanged?.Invoke();
        HandleEquipmentModifier();
    }



}
