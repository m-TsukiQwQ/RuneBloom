using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class UIToolTip : MonoBehaviour
{
    //references
    private RectTransform _rect;
    private Canvas _canvas;
    [SerializeField] protected LayoutElement _layoutElement;

    [Header("Positioning details")]
    [SerializeField] private Vector2 _offset = new Vector2(20, 20);
    protected int _characterWrapLimit;


    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
    }

    protected virtual void Update()
    { 
        

        SetToolTipPosition();
    }

    private void SetToolTipPosition()
    {
        Vector2 pointerPosition = Input.mousePosition;
        float scaleFactor = _canvas.scaleFactor;

        float realWidth = _rect.sizeDelta.x * scaleFactor;
        float realHeight = _rect.sizeDelta.y * scaleFactor;

        //bottom
        float screenBottom = 0f;
        float bottomY = pointerPosition.y - realHeight;

        if (bottomY < screenBottom + (_offset.y * scaleFactor))
            pointerPosition.y = screenBottom + realHeight + (_offset.y * scaleFactor);

        //right
        float screenRight = Screen.width;
        float rightX = pointerPosition.x + realWidth;

        if (rightX > screenRight - (_offset.x * scaleFactor))
            pointerPosition.x = screenRight - realWidth - (_offset.x * scaleFactor);


        _rect.position = pointerPosition;
    }






}
