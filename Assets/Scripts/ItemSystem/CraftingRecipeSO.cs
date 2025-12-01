using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IngredientItem
{
    public ItemDataSO ItemData;
    public int amount;
}

[CreateAssetMenu(menuName = "Game Setup/Craft recipe", fileName = "Craft recipe - ")]
public class CraftingRecipeSO : ScriptableObject
{
    public List<IngredientItem> ingredients;

    public ItemDataSO craftItem;
    public int quantity;
}
