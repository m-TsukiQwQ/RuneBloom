using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public string worldName;
    public string saveDate;
    public long worldTimeTicks;
    public int dayCount;

    public int worldSeed;
    public List<Vector2Int> removedWorldObjects;
    public List<PlacedObjectSaveData> createdWorldObjects;


    public Vector3 playerPosition;


    public List<InventoryItemSaveData> playerInventory;

    //public PlayerStatsSaveData playerStats;

    public List<ChestSaveData> chests;
    public List<EnemySaveData> enemies;

    public GameData()
    {
        this.worldName = "New Game";
        this.saveDate = "";
        this.worldTimeTicks = 0;
        this.dayCount = 1;


        this.worldSeed = 0;
        this.removedWorldObjects = new List<Vector2Int>();
        this.createdWorldObjects = new List<PlacedObjectSaveData>();

        this.playerPosition = Vector3.zero;
        this.playerInventory = new List<InventoryItemSaveData>();

       // this.playerStats = new PlayerStatsSaveData();

        this.chests = new List<ChestSaveData>();
        this.enemies = new List<EnemySaveData>();
    }
}

[Serializable]
public class PlacedObjectSaveData
{
    public string objectID;
    public Vector3 position;
    public string instanceID;

    public PlacedObjectSaveData(string id, Vector3 pos, string guid = null)
    {
        objectID = id;
        position = pos;
        instanceID = guid;
    }
}

//[System.Serializable]
//public class PlayerStatsSaveData
//{
//    // Vitals 
//    public float currentHealth;
//    public float currentHunger;
//    public float currentMagic;

//    // Permanent Base Stats (Progression)
//    // We use a List so we can iterate through your StatType enum dynamically
//    public List<StatSaveEntry> baseStats;

//    public PlayerStatsSaveData()
//    {
//        // Set safe defaults to prevent death loops on new game
//        currentHealth = 100f;
//        currentHunger = 150f;
//        currentMagic = 50f;
//        baseStats = new List<StatSaveEntry>();
//    }
//}

[System.Serializable]
public class StatSaveEntry
{
    public StatType stat; // Ensure your StatType Enum is visible to this script
    public float value;

    public StatSaveEntry(StatType statType, float val)
    {
        stat = statType;
        value = val;
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

