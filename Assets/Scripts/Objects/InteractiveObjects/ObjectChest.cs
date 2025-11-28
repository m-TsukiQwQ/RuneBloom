using NUnit.Framework.Interfaces;
using UnityEngine;

public class ObjectChest : InventoryBase, IInteractable
{
    [SerializeField] private UIChest _chestUI;


    public void Intercat()
    {
        Debug.Log("Interactions");
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
        base.Awake();
        _chestUI = FindFirstObjectByType<UIChest>(FindObjectsInactive.Include);
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            if (_chestUI != null) _chestUI.Hide();
        }
    }

    private void Update()
    {
        
    }
}
