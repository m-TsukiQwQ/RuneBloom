using InputSystem;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [field: SerializeField] public BuildableItemSO ActiveBuildable { get; private set; }

    [SerializeField] private float _maxBuildingDistance = 3;

    [SerializeField] private ConstructionLayer _constructionLayer;
    [SerializeField] private MouseUser _mouseUser;

    private void Update()
    {
        if (!IsMouseWithinBuildableRange()) return;
        if(_mouseUser.IsMouseButtonPressed(MouseButton.Right) && ActiveBuildable != null && _constructionLayer != null)
        {
            _constructionLayer.Build(_mouseUser.mouseInWorldPosition, ActiveBuildable);
        }
    }

    private bool IsMouseWithinBuildableRange()
    {
        return Vector3.Distance(_mouseUser.mouseInWorldPosition, transform.position) <= _maxBuildingDistance;
    }
}
