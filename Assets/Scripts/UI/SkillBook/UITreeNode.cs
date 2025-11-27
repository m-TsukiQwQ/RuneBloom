using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UIManager _ui;
    private UISkillPage _skillPage;

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


    public static event Action<SkillDataSo> OnSkillUpgrade;
    public static event Action<SkillDataSo> OnSkillRemove;

    public event Action<int> OnSkillUpgradeRemovePoints;

    private void Awake()
    {
        _ui = GetComponentInParent<UIManager>();
        _skillPage = GetComponentInParent<UISkillPage>(true);

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

    public int GetCurrentPoints() => _skillPage.GetPoints();

    private bool CanBeUpgraded()
    {
        if (!isUnlocked)
            return false;

        if (_skillData.maximumLevel <= currentNodeLevel)
            return false;

        if(GetCurrentPoints() < _skillData.cost)
            return false;




        return true;
    }
    private bool CanBeUnlocked()
    {
        if (isUnlocked || isLocked)
            return false;

        if (GetCurrentPoints() < _skillData.cost)
            return false;

        foreach (var node in neededNodes)
        {
            if (!node.isUnlocked)
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
        OnSkillUpgrade?.Invoke(_skillData);
        OnSkillUpgradeRemovePoints?.Invoke(_skillData.cost);

    }
    private void Upgrade()
    {
        currentNodeLevel++;
        SetUpgradeLevelText(currentNodeLevel);
        if (currentNodeLevel >= _skillData.maximumLevel)
            isFullyUnlocked = true;

        _ui.skillToolTip.ShowSkillToolTip(_skillData, this);
        OnSkillUpgrade?.Invoke(_skillData);
        OnSkillUpgradeRemovePoints?.Invoke(_skillData.cost);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.skillToolTip.ShowSkillToolTip(_skillData, this);
        _ui.ShowSkillToolTip(true);

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
        _ui.ShowSkillToolTip(false);

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

    private void OnDisable()
    {
        _ui.ShowSkillToolTip(false);
        _bacground.sprite = _unselectedSprite;
    }
}
