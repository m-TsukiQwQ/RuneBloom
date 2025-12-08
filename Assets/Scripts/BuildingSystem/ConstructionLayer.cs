using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ConstructionLayer : TilemapLayer
{
    private Dictionary<Vector3Int, Buildable> _buildables = new();

    public void Build(Vector3 worldCoords, BuildableItemSO item)
    {
        Vector3Int coords = _tilemap.WorldToCell(worldCoords);
        Buildable buildable = new Buildable(item, coords, _tilemap);
        if(item.tile != null)
        {
            TileChangeData tileChangeData = new TileChangeData(coords, item.tile, Color.white, Matrix4x4.Translate(vector: item.tileOffset));
            _tilemap.SetTile(tileChangeData, false);
        }

        _buildables.Add(coords, buildable);
    }


    public bool IsEmpty(Vector3 worldCoords)
    {
        Vector3Int coords = _tilemap.WorldToCell(worldCoords);
        return !_buildables.ContainsKey(coords);
    }
}
