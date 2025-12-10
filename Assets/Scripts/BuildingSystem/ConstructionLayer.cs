using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ConstructionLayer : TilemapLayer
{
    private Dictionary<Vector3Int, Buildable> _buildables = new();

    public void Build(Vector3 worldCoords, BuildableItemSO item)
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
}
