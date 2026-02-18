using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ConstructionLayer : TilemapLayer, ISaveable
{
    private Dictionary<Vector3Int, Buildable> _buildables = new();

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
 
            var saveable = itemObject.GetComponent<SaveableEntity>();
            if (saveable != null)
            {

                if (!string.IsNullOrEmpty(loadId))
                {
                    saveable.RestoreId(loadId);
                }
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

            string typeId = buildable.buildableType.name;


            string instanceId = null;


            if (buildable.gameObject != null)
            {
                var saveable = buildable.gameObject.GetComponent<SaveableEntity>();
                if (saveable != null) instanceId = saveable.Id;
            }


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
