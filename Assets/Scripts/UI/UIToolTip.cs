using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class UIToolTip : MonoBehaviour
{
    [Header("References")]
    protected LayoutElement _layoutElement;
    private RectTransform _rect;
    private Canvas _canvas;

    [Header("Positioning details")]
    [SerializeField] private Vector2 _offset = new Vector2(20, 20);
    protected int _characterWrapLimit;


    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _layoutElement = GetComponent<LayoutElement>();
    }

    protected virtual void Update()
    {

        SetToolTipPosition();

    }

    protected void SetToolTipPosition()
    {
        Vector2 pointerPosition = Input.mousePosition;
        float scaleFactor = _canvas.scaleFactor;

        float realWidth = _rect.sizeDelta.x * scaleFactor;
        float realHeight = _rect.sizeDelta.y * scaleFactor;

        //bottom check
        float screenBottom = 0f;
        float bottomY = pointerPosition.y - realHeight;

        if (bottomY < screenBottom + (_offset.y * scaleFactor))
            pointerPosition.y = screenBottom + realHeight + (_offset.y * scaleFactor);

        //right check
        float screenRight = Screen.width;
        float rightX = pointerPosition.x + realWidth;

        if (rightX > screenRight - (_offset.x * scaleFactor))
            pointerPosition.x = screenRight - realWidth - (_offset.x * scaleFactor);


        _rect.position = pointerPosition;
    }

    protected string GetColoredText(string text, string color)
    {
        return ($"<color={color}> {text} </color>");
    }




}
