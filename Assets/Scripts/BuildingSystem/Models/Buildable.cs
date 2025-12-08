using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class Buildable
{
    [field: SerializeField] public Tilemap parentTileMap {  get; private set; }

    [field: SerializeField] public BuildableItemSO buildableType { get; private set; }

    [field: SerializeField] public GameObject gameObject { get; private set; }

    [field: SerializeField]public Vector3Int coordinates { get; private set; }

    public Buildable (BuildableItemSO type, Vector3Int coords, Tilemap tilemap, GameObject gameObj = null)
    {
        parentTileMap = tilemap;
        buildableType = type;
        coordinates = coords;
        gameObject = gameObj;

    }
}
