using UnityEngine;

public class ObjectChest : InventoryBase, IInteractable, ISaveable
{

    [Header("Save Settings")]
    [SerializeField] private ItemDatabaseSO _database; // Needed to load items back
    private SaveableEntity _saveableEntity;
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
        _saveableEntity = GetComponent<SaveableEntity>();


    }

    protected override void Start()
    {
        base.Start();
        // 1. REGISTER DYNAMICALLY
        // Because this chest was spawned by a building system, it wasn't in the scene 
        // when SaveManager.Start() ran. We must register now.
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.RegisterSaveable(this);

            // 2. CATCH UP DATA
            // If the game is already running and data is loaded, we need to load OUR data immediately.
            if (SaveManager.Instance.HasLoadedData)
            {
                LoadData(SaveManager.Instance.CurrentGameData);
            }
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

    private void OnDestroy()
    {
        // Clean up when destroyed (e.g., changing scenes or destroying chest)
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.UnregisterSaveable(this);
        }
    }
    public void SaveData(ref GameData data)
    {
        string myID = _saveableEntity != null ? _saveableEntity.Id : null;

        if (string.IsNullOrEmpty(myID)) return;

        ChestSaveData chestData = data.chests.Find(x => x.chestID == myID);

        if (chestData == null)
        {
            chestData = new ChestSaveData(myID);
            data.chests.Add(chestData);
        }

        chestData.items.Clear();

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
        string myID = _saveableEntity != null ? _saveableEntity.Id : null;
        if (string.IsNullOrEmpty(myID)) return;

        ChestSaveData chestData = data.chests.Find(x => x.chestID == myID);

        if (chestData != null)
        {
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

        }
    }
}
