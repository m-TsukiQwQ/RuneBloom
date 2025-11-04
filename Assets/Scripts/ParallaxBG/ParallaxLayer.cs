using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform _background;
    [SerializeField] private float _parallaxMultiplier;
    [SerializeField] private float _imageWidthOffset = 10;

    private float _imageFullWidth;
    private float _imageHalfWidth;

    public void CalculateImageWidth()
    {
        _imageFullWidth = _background.GetComponent<SpriteRenderer>().bounds.size.x;
        _imageHalfWidth = _imageFullWidth / 2;

    }

    public void Move(float distanceToMove)
    {
        _background.position += Vector3.right * (distanceToMove * _parallaxMultiplier);
    }

    public void LoopBackground(float cameraLeftEdge, float cameraRightEdge)
    {
        float imageRightEdge = (_background.position.x + _imageHalfWidth) - _imageWidthOffset;
        float imageLeftEdge = (_background.position.x - _imageHalfWidth) + _imageWidthOffset;

        if (imageRightEdge < cameraLeftEdge)
            _background.position += Vector3.right * _imageFullWidth;
        else if (imageLeftEdge > cameraRightEdge)
            _background.position += Vector3.right * -_imageFullWidth;

    }           
}
