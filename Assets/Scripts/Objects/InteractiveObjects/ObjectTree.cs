using UnityEngine;

public class ObjectTree : ObjectBase
{
    [SerializeField] private Transform _stumpPosition;
    [SerializeField] private GameObject _stumpPrefab;


    protected override void DecreaseHealth(float health)
    {
        base.DecreaseHealth(health);
        if (_currentHealth <= 0)
        {
            if (_stumpPrefab != null)
                Instantiate(_stumpPrefab, _stumpPosition.position, Quaternion.identity);
            

        }
    }

    private void OnDestroy()
    {
        
    }









}
