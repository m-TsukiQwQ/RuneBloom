using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public float timeOfDay;
    public int dayCount;

    public int worldSeed;
    public List<Vector2Int> removedWorldObjects;
    public List<PlacedObjectSaveData> createdWorldObjects;


    public Vector3 playerPosition;
    public int playerHealth;

    public List<InventoryItemSaveData> playerInventory;

    public List<ChestSaveData> chests;
    public List<EnemySaveData> enemies;

    public GameData()
    {
        this.timeOfDay = 0f;
        this.dayCount = 1;


        this.worldSeed = 0;
        this.removedWorldObjects = new List<Vector2Int>();
        this.createdWorldObjects = new List<PlacedObjectSaveData>();

        this.playerHealth = 100;
        this.playerPosition = Vector3.zero;
        this.playerInventory = new List<InventoryItemSaveData>();

        this.chests = new List<ChestSaveData>();
        this.enemies = new List<EnemySaveData>();
    }
}

[Serializable]
public class PlacedObjectSaveData
{
    public string objectID;
    public Vector3 position;

    public PlacedObjectSaveData(string id, Vector3 pos)
    {
        objectID = id;
        position = pos;
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

[System.Serializable]
public class EnemySaveData
{
    public string enemyID;
    public Vector3 position;
    public bool isDead;

    public EnemySaveData(string id, Vector3 pos, bool dead)
    {
        enemyID = id;
        position = pos;
        isDead = dead;
    }
}

