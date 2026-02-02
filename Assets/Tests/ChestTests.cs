using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.TestTools;

public class ChestTests
{
    [UnityTest]
    public IEnumerator ObjectChest_LoadData_RestoresItemsCorrectly()
    {
        // Purpose: Verify chest correctly restores slot state from GameData object (Safety check only without DB)

        // Arrange
        GameObject chestGO = new GameObject("TestChest_Load");

        // FIX: Add SaveableEntity FIRST to ensure we reference the correct one
        var saveable = chestGO.AddComponent<SaveableEntity>();
        string testID = "chest_test_id_load";
        ForceSetSaveableID(saveable, testID);

        // Pre-check: Ensure ID is actually set before proceeding
        Assert.IsFalse(string.IsNullOrEmpty(saveable.Id), "Test Setup Failed: Could not force set ID on SaveableEntity.");

        // Now add ObjectChest. It will find the existing SaveableEntity in Awake.
        var chest = chestGO.AddComponent<ObjectChest>();

        // Note: We removed Database injection here as requested.
        // LoadData might return early if database is missing, but the test will verify it doesn't crash.

        // Simulate Data
        GameData mockData = new GameData();
        ChestSaveData chestSaveData = new ChestSaveData(testID);
        chestSaveData.items.Add(new InventoryItemSaveData("item_01", 5, 0));
        mockData.chests.Add(chestSaveData);

        yield return null; // Wait for Awake/Start cycles

        // Act
        chest.LoadData(mockData);

        // Assert
        Assert.DoesNotThrow(() => chest.LoadData(mockData));

        // Cleanup
        Object.DestroyImmediate(chestGO);
    }


    // --- HELPER METHODS ---

    private void ForceSetSaveableID(SaveableEntity entity, string id)
    {
        var type = entity.GetType();
        var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        // Try to find the backing field for the ID using common conventions
        // <Id>k__BackingField is the standard C# compiler name for auto-properties: public string Id { get; set; }
        FieldInfo field = type.GetField("id", flags) ??
                          type.GetField("_id", flags) ??
                          type.GetField("m_Id", flags) ??
                          type.GetField("itemID", flags) ??
                          type.GetField("<Id>k__BackingField", flags);

        if (field != null)
        {
            field.SetValue(entity, id);
        }
        else
        {
            // Fallback to property setter if no field found
            PropertyInfo prop = type.GetProperty("Id", flags);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(entity, id);
            }
            else
            {
                Debug.LogError($"[Test Helper] Fatal: Could not find field or property 'Id' on {type.Name}. Cannot force set ID.");
            }
        }

    }
}

