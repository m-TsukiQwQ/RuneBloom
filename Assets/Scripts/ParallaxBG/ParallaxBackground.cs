using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] float _cameraSpeed;
    private Camera _mainCamera;
    [SerializeField] private ParallaxLayer[] _backgroundLayers;
    private float _lastCameraPositionX;

    private float _cameraHalfWidth;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _cameraHalfWidth = _mainCamera.orthographicSize * _mainCamera.aspect;
        CalculateImageLength();
    }

    private void FixedUpdate()
    {
        _mainCamera.transform.position += Vector3.right * _cameraSpeed * Time.fixedDeltaTime;
        float currentCameraPositionX = _mainCamera.transform.position.x;
        float distanceToMove = currentCameraPositionX - _lastCameraPositionX;
        _lastCameraPositionX = currentCameraPositionX;

        float cameraLeftEdge = currentCameraPositionX - _cameraHalfWidth;
        float cameraRightEdge = currentCameraPositionX + _cameraHalfWidth;

        foreach (ParallaxLayer layer in _backgroundLayers)
        {
            layer.Move(distanceToMove);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void CalculateImageLength()
    {
        foreach (ParallaxLayer layer in _backgroundLayers)
        {
            layer.CalculateImageWidth();
        }
    }
}
