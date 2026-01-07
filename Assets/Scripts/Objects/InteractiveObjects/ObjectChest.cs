using NUnit.Framework.Interfaces;
using UnityEngine;

public class ObjectChest : InventoryBase, IInteractable, ISaveable
{

    [Header("Save Settings")]
    [SerializeField] private ItemDatabaseSO _database; // Needed to load items back

    [SerializeField] private UIChest _chestUI;

    private string _id;


    public void Intercat()
    {

        ToggleChest();
    }

    private void ToggleChest()
    {
        if (_chestUI == null) return;

        // LOGIC FIX:
        // 1. Is the UI open?
        // 2. Is it currently displaying THIS chest?
        if (_chestUI.gameObject.activeSelf && _chestUI.CurrentChest == this)
        {
            // We are clicking the chest that is already open -> Close it
            _chestUI.Hide();
        }
        else
        {
            // We are clicking a new chest (or UI was closed) -> Open/Switch to this one
            _chestUI.Show(this);

            // Optional: Open Player Inventory too
            // FindFirstObjectByType<UIInventory>(FindObjectsInactive.Include)?.Show(); 
        }
    }


    protected override void Awake()
    {
        base.Awake(); // Initialize slots
        _chestUI = FindFirstObjectByType<UIChest>(FindObjectsInactive.Include);

        // FIX: Get the unique ID from the SaveableEntity component
        SaveableEntity saveable = GetComponent<SaveableEntity>();
        if (saveable != null)
        {
            _id = saveable.Id;
        }
        else
        {
            Debug.LogError($"ObjectChest '{gameObject.name}' is missing a SaveableEntity component!");
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            if (_chestUI != null) _chestUI.Hide();
        }
    }

    private void OnDisable()
    {
        
    }

    public void SaveData(ref GameData data)
    {
        // 1. Find if this chest already exists in the save file
        ChestSaveData chestData = data.chests.Find(x => x.chestID == _id);

        // 2. If not, create a new entry and add it
        if (chestData == null)
        {
            chestData = new ChestSaveData(_id);
            data.chests.Add(chestData);
        }

        // 3. Clear old items in the save file for this specific chest
        chestData.items.Clear();

        // 4. Loop and Save
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].HasItem)
            {
                InventoryItemSaveData itemToSave = new InventoryItemSaveData(
                    slots[i].itemData.itemID,
                    slots[i].stackSize,
                    i
                );
                chestData.items.Add(itemToSave);
            }
        }
    }

    public void LoadData(GameData data)
    {
        // 1. Find my data using my ID
        ChestSaveData chestData = data.chests.Find(x => x.chestID == _id);

        // 2. If found, load items
        if (chestData != null)
        {
            // Clear current state first
            foreach (InventorySlot slot in slots) slot.Clear();

            foreach (InventoryItemSaveData savedItem in chestData.items)
            {
                if (savedItem.slotIndex < slots.Length)
                {
                    ItemDataSO itemRef = _database.GetItemByID(savedItem.itemID);
                    if (itemRef != null)
                    {
                        slots[savedItem.slotIndex].AssignItem(itemRef, savedItem.stackSize);
                    }
                }
            }

            // Update UI if this chest is currently open
            //OnInventoryChanged?.Invoke();
        }
    }
}
