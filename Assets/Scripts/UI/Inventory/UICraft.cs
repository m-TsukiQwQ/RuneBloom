using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICraft : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private List<CraftingRecipeSO> _allRecipes;
    private List<UICraftingSlot> _slots = new List<UICraftingSlot>();





    [Header("References")]
    [SerializeField] private Transform _container;   // The Grid Panel
    [SerializeField] private GameObject _slotPrefab; // Prefab with CraftingSlotUI

    // Reference to the backend 
    public PlayerInventory playerInventory;

    private void Awake()
    {
       playerInventory = FindFirstObjectByType<PlayerInventory>();
        GenerateRecipes();
    }
    private void Start()
    {
        
        playerInventory.OnInventoryChanged += UpdateUI;
        UpdateUI();
        gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        foreach (var slot in _slots)
        {
            slot.UpdateSlot();
        }
        
    }


    private void GenerateRecipes()
    {
        foreach (Transform child in _container)
        {
            Destroy(child.gameObject);
        }


        foreach (CraftingRecipeSO recipe in _allRecipes)
        {

            GameObject slotObj = Instantiate(_slotPrefab, _container);
            slotObj.transform.localScale = Vector3.one;


            UICraftingSlot slotScript = slotObj.GetComponent<UICraftingSlot>();
            slotScript.Init(recipe, this);
            _slots.Add(slotScript);
        }
    }

    public void OnCraftRequested(CraftingRecipeSO recipe)
    {

        Debug.Log($"Player wants to craft: {recipe.craftItem.itemName}");

        playerInventory.TryToCraft(recipe); 
    }
}
