using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public float timeOfDay;
    public int dayCount;

    public Vector3 playerPosition;
    public int playerHealth;

    public List<InventoryItemSaveData> playerInventory;
    public List<ChestSaveData> chests;

    public GameData()
    {
        this.timeOfDay = 0;
        this.dayCount = 0;

        this.playerPosition = Vector3.zero;

        this.playerInventory = new List<InventoryItemSaveData>();
        this.chests = new List<ChestSaveData>();
    }
}

[Serializable]
public class InventoryItemSaveData
{
    public string itemID;
    public int stackSize;
    public int slotIndex;

    public InventoryItemSaveData(string id, int count, int index)
    {
        itemID = id;
        stackSize = count;
        slotIndex = index;
    }
}

[Serializable]
public class ChestSaveData
{
    public string chestID;
    public List<InventoryItemSaveData> items;

    public ChestSaveData(string id)
    {
        chestID = id;
        items = new List<InventoryItemSaveData>();
    }
}

