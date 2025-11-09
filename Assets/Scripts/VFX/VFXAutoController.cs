using UnityEngine;

public class VFXAutoController : MonoBehaviour
{
    [SerializeField] private bool _autoDestroy = true;
    [SerializeField] private float _destroyDelay = 1;

    [Header("Random rotation")]
    [SerializeField] private float _minRotation;
    [SerializeField] private float _maxRotation;
    [SerializeField] private bool _radomRotation = true;
    private void Start()
    {
        ApplyRandomRotation();
        if (_autoDestroy)
            Destroy(gameObject, _destroyDelay);
    }

    private void ApplyRandomRotation()
    {
        if (_radomRotation == false)
        {
            return;
        }
        float zRotation = Random.Range(_minRotation, _maxRotation);
        transform.Rotate(0, 0, zRotation);
    }

}
