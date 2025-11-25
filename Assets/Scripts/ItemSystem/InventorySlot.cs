using System;
using System.Linq;

[Serializable]
public class InventorySlot
{
    public ItemDataSO itemData;
    public int stackSize = 1;

    public ItemModifier[] modifiers { get; private set; }
    private ItemModifier[] _activeRuntimeModifiers;
    private string _activeSourceName;


    public InventorySlot(ItemDataSO itemData, int amount)
    {
        AssignItem(itemData, amount);   
    }

    public void AssignItem(ItemDataSO itemData, int amount)
    {
        this.itemData = itemData;
        stackSize = amount;


        if (itemData is EquipmentDataSO equip)
        {
            if (equip.modifiers != null)
                modifiers = equip.modifiers.ToArray(); // Copy array to prevent referencing SO directly
            else
                modifiers = new ItemModifier[0];
        }
        else
        {
            modifiers = null;
            modifiers = null;

        }
    }

    public InventorySlot() // Empty Constructor
    {
        Clear();
    }

    public void Clear()
    {
        itemData = null;
        stackSize = 0;
        modifiers = null;
        
    }

    public void ManageModifier(EntityStats playerStats)
    {
        if (_activeRuntimeModifiers != null)
        {
            RemoveModifiers(playerStats);
        }

        if(itemData != null)
        {
            AddModifiers(playerStats);
        }
    }


    public void AddModifiers(EntityStats playerStats)
    {
        if (itemData is EquipmentDataSO equip)
        {
            if (equip.modifiers != null)
            {
                modifiers = equip.modifiers;
                _activeRuntimeModifiers = modifiers.ToArray();
            }
            else
                modifiers = new ItemModifier[0];
        }

        if (modifiers == null || modifiers.Length == 0)
            return;

        _activeRuntimeModifiers = new ItemModifier[modifiers.Length];
        _activeSourceName = itemData != null ? itemData.equipmentType.ToString() : "Unknown";


        for (int i = 0; i < modifiers.Length; i++)
        {
            var modData = modifiers[i];

            if (modData == null || modData.statType == StatType.None) 
                continue;

            Stat statToModify = playerStats.GetStatByType(modData.statType);

            if (statToModify == null) 
                continue;

            if (modData.isMultiplier)
                statToModify.AddMultiplier(modData.value, _activeSourceName); 
            else
                statToModify.AddModifier(modData.value, _activeSourceName); 

            _activeRuntimeModifiers[i] = modData;
        }
    }

    public void RemoveModifiers(EntityStats playerStats)
    {

        if (_activeRuntimeModifiers == null || _activeRuntimeModifiers.Length == 0) 
            return;


        string sourceToRemove = _activeSourceName;
        if (string.IsNullOrEmpty(sourceToRemove)) 
            sourceToRemove = "Unknown";


        for (int i = 0; i < _activeRuntimeModifiers.Length; i++)
        {
            ItemModifier modData = _activeRuntimeModifiers[i];

            if (modData == null || modData.statType == StatType.None) 
                continue;

            Stat statToModify = playerStats.GetStatByType(modData.statType);

            if (statToModify != null)
            {
                if (modData.isMultiplier)
                    statToModify.RemoveMultiplier(sourceToRemove);
                else
                    statToModify.RemoveModifier(sourceToRemove);
            }
        }

        _activeRuntimeModifiers = null;
        _activeSourceName = null;
    }

    public bool HasItem => itemData != null;
    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    public void AddStack(int amount) => stackSize += amount;
    public void RemoveStack(int amount) => stackSize -= amount;
}
