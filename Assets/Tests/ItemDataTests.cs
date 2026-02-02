using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemDataTests
{
    // --- ITEM DATA (SCRIPTABLE OBJECT) TESTS ---

    [Test]
    public void ItemData_RegenerateID_SetsIDToGUID()
    {
        // Purpose: Verify that the context menu option 'RegenerateID' attempts to set a unique ID.
        // This test validates Editor-side logic (Assets/Scripts/Inventory/ItemDataSO.cs).

#if UNITY_EDITOR
        // Arrange
        ItemDataSO item = ScriptableObject.CreateInstance<ItemDataSO>();

        // FIX: Create a temporary asset file because RegenerateID relies on AssetDatabase finding a file path.
        // In-memory objects have no path, so AssetPathToGUID returns empty string.
        string tempPath = "Assets/Temp_Test_Item_" + System.Guid.NewGuid() + ".asset";
        AssetDatabase.CreateAsset(item, tempPath);

        item.itemID = "old_id_value";

        // Act
        item.RegenerateID();

        // Assert
        Assert.AreNotEqual("old_id_value", item.itemID, "RegenerateID should change the ID from the old value.");
        Assert.IsFalse(string.IsNullOrEmpty(item.itemID), "RegenerateID should not result in an empty ID when asset exists on disk.");

        // Cleanup
        AssetDatabase.DeleteAsset(tempPath);
#else
        Assert.Ignore("This test relies on AssetDatabase and must run in the Unity Editor.");
#endif
    }
}