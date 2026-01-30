using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InventoryTests
{
    private GameObject _playerObj;
    private PlayerInventory _inventory; 
    private ItemDataSO _testItem;

    [SetUp]
    public void Setup()
    {
        // 1. Create the Player
        _playerObj = new GameObject("Player");
        _inventory = _playerObj.AddComponent<PlayerInventory>();

        _inventory.maxInventorySize = 20;
        _inventory.slots = new InventorySlot[20];
        for (int i = 0; i < _inventory.slots.Length; i++)
        {
            _inventory.slots[i] = new InventorySlot();
        }

        // 3. Create a Dummy Item
        _testItem = ScriptableObject.CreateInstance<ItemDataSO>();
        _testItem.itemID = "test_item";
        _testItem.itemName = "Test Apple";
        _testItem.maxStackSize = 10;
    }

    [TearDown]
    public void Teardown()
    {

        Object.DestroyImmediate(_playerObj);
        Object.DestroyImmediate(_testItem);
    }

    [UnityTest]
    public IEnumerator Inventory_AddItem_IncreasesStack()
    {
        // Act: Add 5 items
        bool added = _inventory.TryAddItem(_testItem, 5);

        yield return null;

        // Assert
        Assert.IsTrue(added, "TryAddItem should return true");
        Assert.AreEqual(5, _inventory.slots[0].stackSize, "Slot 0 should have 5 items");
        Assert.AreEqual(_testItem, _inventory.slots[0].itemData, "Slot 0 should contain the correct item");
    }

    [UnityTest]
    public IEnumerator Inventory_FullStack_OverflowsToNextSlot()
    {
        // Act: Add 15 items (Stack size is 10)
        _inventory.TryAddItem(_testItem, 15);

        yield return null;

        // Assert
        Assert.AreEqual(10, _inventory.slots[0].stackSize, "Slot 0 should be full (10)");
        Assert.AreEqual(5, _inventory.slots[1].stackSize, "Slot 1 should hold the overflow (5)");
    }
}