using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Game Setup/Item Data/Buildable item", fileName = "Buildable data - ")]
public class BuildableItemSO : ItemDataSO
{
    [field:SerializeField] public TileBase tile { get; private set; }

    [field: SerializeField] public Vector3 tileOffset;

    [field: SerializeField] public Sprite previewSprite {  get; private set; }

    [field: SerializeField] public GameObject GameObject { get; private set; }
}
