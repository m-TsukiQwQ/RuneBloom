using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UIManager _ui;

    [Header("Information details")]
    public SkillDataSo _skillData;
    [SerializeField] private string _skillName;
    [SerializeField] private TextMeshProUGUI _levelUpgradeTmp;
    public int currentNodeLevel;

    [Header("Appereance configuration")]
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _unselectedSprite;
    [SerializeField] private Image _bacground;
    [SerializeField] private Image _skillIcon;
    private string _lockedColorHex = "#8E8E8E";

    [Header("Unlock details")]
    public UITreeNode[] neededNodes;
    public UITreeNode[] conflictNodes;
    public bool isUnlocked;
    public bool isFullyUnlocked;
    public bool isLocked;


    private void Awake()
    {
        _ui = GetComponentInParent<UIManager>();

        UpdateIconColor(GetColorByHex(_lockedColorHex));
        SetUpgradeLevelText(currentNodeLevel);
    }

    private void OnValidate()
    {
        if (_skillData == null)
            return;

        _skillName = _skillData.skillName;
        _skillIcon.sprite = _skillData.icon;
        gameObject.name = "UI_TreeNode - " + _skillData.skillName;
    }

    private bool CanBeUpgraded()
    {
        if (_skillData.maximumLevel <= currentNodeLevel)
            return false;

        foreach (var node in neededNodes)
        {
            if (!node.isFullyUnlocked)
                return false;
        }

        foreach (var node in conflictNodes)
        {
            if (node.isUnlocked)
            {
                return false;
            }
        }

        return true;
    }
    private bool CanBeUnlocked()
    {
        if (isUnlocked || isLocked)
            return false;

        foreach (var node in neededNodes)
        {
            if (!node.isFullyUnlocked)
                return false;
        }

        foreach (var node in conflictNodes)
        {
            if (node.isUnlocked)
            {
                return false;
            }
        }

        return true;
    }

    private void Unlock()
    {
        isUnlocked = true;
        currentNodeLevel++;
        UpdateIconColor(Color.white);

        foreach (var node in conflictNodes)
            node.isLocked = true;

        _ui.skillToolTip.ShowSkillToolTip(_skillData, this);

    }
    private void Upgrade()
    {
        currentNodeLevel++;
        SetUpgradeLevelText(currentNodeLevel);
        if (currentNodeLevel >= _skillData.maximumLevel)
            isFullyUnlocked = true;

        _ui.skillToolTip.ShowSkillToolTip(_skillData, this);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.ShowToolTip(true);
        _ui.skillToolTip.ShowSkillToolTip(_skillData, this);

        _bacground.sprite = _selectedSprite;

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            Unlock();
        else if (CanBeUpgraded())
        {
            Upgrade();
        }
        else
            Debug.Log("Cannot be unlocked");
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.ShowToolTip(false);

        _bacground.sprite = _unselectedSprite;

    }


    private void UpdateIconColor(Color color)
    {
        if (_skillIcon == null)
            return;

        _skillIcon.color = color;
    }

    private Color GetColorByHex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }

    private void SetUpgradeLevelText(int level)
    {
        if (_levelUpgradeTmp != null)
        {

            if (level > 1)
            {
                _levelUpgradeTmp.text = level.ToString();
                _levelUpgradeTmp.gameObject.SetActive(true);
            }
            else
            {

                _levelUpgradeTmp.gameObject.SetActive(false);
            }
        }


    }
}
