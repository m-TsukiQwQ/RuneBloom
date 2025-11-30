using UnityEngine;

public class ObjectTree : ObjectBase
{
    [SerializeField] private Transform _stumpPosition;
    [SerializeField] private GameObject _stumpPrefab;


    private void OnDestroy()
    {
        if (_stumpPrefab != null)
            Instantiate(_stumpPrefab, _stumpPosition.position, Quaternion.identity);
    }









}
