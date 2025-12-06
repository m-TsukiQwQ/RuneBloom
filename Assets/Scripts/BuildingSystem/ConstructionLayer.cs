using UnityEngine;


public class ConstructionLayer : TilemapLayer
{
    public void Build(Vector3 worldCoords, BuildableItemSO item)
    {
        var coords = _tilemap.WorldToCell(worldCoords);
        if(item.tile != null)
        {
            _tilemap.SetTile(coords, item.tile);
        }
    }
}
