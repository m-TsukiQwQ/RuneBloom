using System.Text;
using TMPro;
using UnityEngine;

public class UICraftItemToolTip : UIToolTip
{
    [Header("Configuration")]
    [SerializeField] private TextMeshProUGUI _itemNameTMP;
    [SerializeField] private TextMeshProUGUI _itemDescriptionTMP;
    [SerializeField] private TextMeshProUGUI _ingredientsTMP;
    [SerializeField] private TextMeshProUGUI _itemStatsTMP;
    [SerializeField] private TextMeshProUGUI _itemType;

    [SerializeField] private GameObject _itemDescription;
    [SerializeField] private GameObject _itemStats;

    [Header("Color configuration")]
    [SerializeField] private string _typeHex;
    [SerializeField] private string _statsHex;
    [SerializeField] private string _importantInforamtionHex;
    [SerializeField] private string _metConditionHex;
    [SerializeField] private string _notMetConditionHex;
    [SerializeField] private Color exampleColor;

    public void ShowCraftItemToolTip( CraftingRecipeSO recipe,  UICraftingSlot slot)
    {
        SetToolTipPosition();
        _itemNameTMP.text = $"{recipe.craftItem.itemName}";

        _itemStatsTMP.text = GetStats(recipe.craftItem);
        _itemStats.gameObject.SetActive(_itemStatsTMP.text.Length == 0 ? false : true);

        _ingredientsTMP.text = GetIngredients(recipe, slot);
        _itemDescriptionTMP.text = $"{recipe.craftItem.description}";
        _itemDescription.gameObject.SetActive(_itemDescriptionTMP.text.Length == 0 ? false : true);

        _itemType.text = GetColoredText($"{recipe.craftItem.itemDisplayType.ToString()}", _typeHex);

    }

    private string GetIngredients(CraftingRecipeSO recipe, UICraftingSlot slot)
    {
        StringBuilder sb = new StringBuilder();

        if(recipe.craftItem == null) 
            return sb.ToString();

        sb.AppendLine("Ingredients: ");
        foreach(var ingredient in recipe.ingredients)
        {
            string ingredientColor = slot.uiCraft.playerInventory.GetTotalAmount(ingredient.ItemData) > ingredient.amount ? _metConditionHex : _notMetConditionHex;
            sb.AppendLine(GetColoredText($" - {ingredient.amount} {ingredient.ItemData.itemName}(s)", ingredientColor));
        }

        return sb.ToString();
    }

    public string GetStats(ItemDataSO itemData)
    {
        StringBuilder sb = new StringBuilder();

        if (itemData is EquipmentDataSO equipmentData)
        {
            foreach (var mod in equipmentData.modifiers)
            {
                sb.AppendLine(GetColoredText($"+ {mod.value} {mod.statType.ToString()}", _statsHex));
            }

        }

        if (itemData is InstrumentDataSO instrumentData)
        {
            foreach (var mod in instrumentData.modifiers)
            {
                sb.AppendLine(GetColoredText($"+ {mod.value} {mod.statType.ToString()}", _statsHex));
            }

        }

        return sb.ToString();
    }
}
