using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UIManager _ui;
    private RectTransform _rect;

    [SerializeField] private SkillDataSo _skillData;
    [SerializeField] private string _skillName;

    [SerializeField] private Image _skillIcon;
    private string _lockedColorHex = "#8E8E8E";
    private Color _lastColor;
    public bool _isUnlocked;

    private void Awake()
    {
        _ui = GetComponentInParent<UIManager>();
        _rect = GetComponent<RectTransform>();

        UpdateIconColor(GetColorByHex(_lockedColorHex));

    }

    private void OnValidate()
    {
        if (_skillData == null)
            return;
        _skillName = _skillData.skillName;
        _skillIcon.sprite = _skillData.icon;
        gameObject.name = "UI_TreeNode - " + _skillData.skillName;
    }
    private bool CanBeUnlocked()
    {
        if (_isUnlocked)
            return false;
        return true;
    }

    private void Unlock()
    {
        _isUnlocked = true;
        UpdateIconColor(Color.white);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.ShowToolTip(true);
        _ui.skillToolTip.ShowSkillToolTip(_skillData);
        if (_isUnlocked == false)
            UpdateIconColor(Color.white * .9f);
        Debug.Log("Show tooltip");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            Unlock();
        else
            Debug.Log("Cannot be unlocked");
        Debug.Log("Unlock skill");
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.ShowToolTip(false);
        if (_isUnlocked == false)
            UpdateIconColor(_lastColor);

        Debug.Log("Hide tooltip");
    }


    private void UpdateIconColor(Color color)
    {
        if (_skillIcon == null)
            return;

        _lastColor = _skillIcon.color;
        _skillIcon.color = color;
    }

    private Color GetColorByHex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }
}
