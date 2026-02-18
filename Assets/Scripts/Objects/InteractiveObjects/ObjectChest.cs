using UnityEngine;

public class ObjectChest : InventoryBase, IInteractable, ISaveable
{

    [Header("Save Settings")]
    [SerializeField] private ItemDatabaseSO _database; 
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


        if (_chestUI.gameObject.activeSelf && _chestUI.CurrentChest == this)
        {
            _chestUI.Hide();
        }
        else
        {
            _chestUI.Show(this);

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
       
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.RegisterSaveable(this);

            
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
        // Clean up when destroyed ( changing scenes or destroying chest)
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
