using InputSystem;
using System;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [field: SerializeField] public BuildableItemSO activeBuildable { get; private set; }

    [SerializeField] private float _maxBuildingDistance = 3;

    [SerializeField] private ConstructionLayer _constructionLayer;
    [SerializeField] private MouseUser _mouseUser;

    [SerializeField] private PreviewLayer _previewLayer;

    public event Action OnPlaceSuccess;


    private void Update()
    {
        var mousePos = _mouseUser.mouseInWorldPosition;
        if(!IsMouseWithinBuildableRange())
        {
            _previewLayer.ClearPreview();
            return;
        }

        if (activeBuildable == null)
        {
            _previewLayer.ClearPreview();
            return;
        }

        _previewLayer.ShowPreview(activeBuildable, mousePos, _constructionLayer.IsEmpty(mousePos));



        if(_mouseUser.IsMouseButtonPressed(MouseButton.Right)  && _constructionLayer != null && _constructionLayer.IsEmpty(mousePos)) 
        {
            _constructionLayer.Build(mousePos, activeBuildable);
            OnPlaceSuccess?.Invoke();
        }
    }



    public void ChangeBuildable(BuildableItemSO buildable)
    {
        activeBuildable = buildable;
    }

    private bool IsMouseWithinBuildableRange()
    {
        bool withinRange = Vector3.Distance(_mouseUser.mouseInWorldPosition, transform.position) <= _maxBuildingDistance;
        return withinRange;
    }
}
