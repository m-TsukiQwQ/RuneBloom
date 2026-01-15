using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ConstructionLayer : TilemapLayer, ISaveable
{
    private Dictionary<Vector3Int, Buildable> _buildables = new();

    // DRAG ALL YOUR BUILDABLE SOs HERE IN INSPECTOR
    [SerializeField] private List<BuildableItemSO> _allBuildables;

    private void Start()
    {
        // Register if this object exists in the scene at start
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.RegisterSaveable(this);
        }
    }

    private void OnDestroy()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.UnregisterSaveable(this);
        }
    }

    public void Build(Vector3 worldCoords, BuildableItemSO item, string loadId = null)
    {
        GameObject itemObject = null;

        Vector3Int coords = _tilemap.WorldToCell(worldCoords);
        if (item.tile != null)
        {
            TileChangeData tileChangeData = new TileChangeData(coords, item.tile, Color.white, Matrix4x4.Translate(vector: item.tileOffset));
            _tilemap.SetTile(tileChangeData, false);
        }

        else if (item.GameObject != null)
        {
            itemObject = Instantiate(item.GameObject, _tilemap.CellToWorld(coords) + _tilemap.cellSize / 4 + item.tileOffset /2 , Quaternion.identity);
            // --- CRITICAL RESTORE LOGIC ---
            // If we are loading a save, force this object to use the old ID
            var saveable = itemObject.GetComponent<SaveableEntity>();
            if (saveable != null)
            {
                // CASE 1: LOADING GAME (Restore old ID)
                if (!string.IsNullOrEmpty(loadId))
                {
                    saveable.RestoreId(loadId);
                }
                // CASE 2: PLACING NEW OBJECT (Generate Fresh ID)
                // This overwrites the Prefab's hardcoded ID with a unique one for this specific chest instance.
                else
                {
                    saveable.GenerateId();
                }
            }
        }


        Buildable buildable = new Buildable(item, coords, _tilemap, itemObject);
        _buildables.Add(coords, buildable);
    }

    public void Destroy(Vector3 worldCoords)
    {
        Vector3Int coords = _tilemap.WorldToCell(worldCoords);
        if (!_buildables.ContainsKey(coords))
            return;

        var buildable = _buildables[coords];
        _buildables.Remove(coords);
        buildable.Destroy();
    }


    public bool IsEmpty(Vector3 worldCoords)
    {
        Vector3Int coords = _tilemap.WorldToCell(worldCoords);
        return !_buildables.ContainsKey(coords) && _tilemap.GetTile(coords) == null;
    }

    public void SaveData(ref GameData data)
    {
        data.createdWorldObjects.Clear();

        foreach (var kvp in _buildables)
        {
            Buildable buildable = kvp.Value;

            // Use cell position or world position depending on your logic
            Vector3 pos = _tilemap.CellToWorld(kvp.Key) + _tilemap.cellSize / 4;

            // 1. Get the Type ID (e.g. "Chest_Wood")
            // Adjusted to match your Buildable class property: 'buildableType'
            string typeId = buildable.buildableType.name;

            // 2. Get the Instance GUID (e.g. "550e8400-e29b...")
            string instanceId = null;

            // Adjusted to match your Buildable class property: 'gameObject' (lowercase)
            if (buildable.gameObject != null)
            {
                var saveable = buildable.gameObject.GetComponent<SaveableEntity>();
                if (saveable != null) instanceId = saveable.Id;
            }

            // 3. Add to GameData
            PlacedObjectSaveData objData = new PlacedObjectSaveData(typeId, pos, instanceId);
            data.createdWorldObjects.Add(objData);
        }
    }

    public void LoadData(GameData data)
    {
        // 1. Clear current state
        foreach (var kvp in _buildables)
        {
            kvp.Value.Destroy();
        }
        _buildables.Clear();
        _tilemap.ClearAllTiles();

        // 2. Rebuild from Save
        foreach (var objData in data.createdWorldObjects)
        {
            // Find the correct SO by name
            BuildableItemSO itemType = _allBuildables.Find(x => x.name == objData.objectID);

            if (itemType != null)
            {
                // Call Build with the saved GUID
                Build(objData.position, itemType, objData.instanceID);
            }
        }
    }
}
