using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Game Setup/Item Data/Buildable item", fileName = "Buildable data - ")]
public class BuildableItemSO : ItemDataSO
{
    [field:SerializeField] public TileBase tile { get; private set; }
}
