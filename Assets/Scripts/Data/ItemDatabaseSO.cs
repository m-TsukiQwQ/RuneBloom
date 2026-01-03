using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Game Setup/Item Database", fileName = "Item Database ")]
public class ItemDatabaseSO : ScriptableObject
{

    public List<ItemDataSO> allItems;

    private Dictionary<string, ItemDataSO> _itemLookup;


    private void Init()
    {
        _itemLookup = new Dictionary<string, ItemDataSO>();

        foreach (var item in allItems)
        {

            if (item != null && !string.IsNullOrEmpty(item.itemID))
            {
                if (!_itemLookup.ContainsKey(item.itemID))
                {
                    _itemLookup.Add(item.itemID, item);
                }
                else
                {
                    Debug.LogWarning($"Duplicate Item ID found in Database: {item.itemID}. Check your items!");
                }
            }
        }
    }

    public ItemDataSO GetItemByID(string id)
    {

        if (_itemLookup == null) Init();

        if (_itemLookup.TryGetValue(id, out ItemDataSO item))
        {
            return item;
        }

        Debug.LogError($"Could not find item with ID: {id}. Did you forget to add it to the Database?");
        return null;
    }
}