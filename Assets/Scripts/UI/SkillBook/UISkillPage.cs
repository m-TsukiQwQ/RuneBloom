using TMPro;
using UnityEngine;

public class UISkillPage : MonoBehaviour
{
    public UISkillBookPageType type;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private SkillManager _skillManager;

    private UITreeNode[] _childNodes;

    private void Awake()
    {
        _skillManager = FindFirstObjectByType<SkillManager>();
        _childNodes = GetComponentsInChildren<UITreeNode>(includeInactive: true);

        UpdateSkillPointsText();

    }

    private void Update()
    {
        UpdateSkillPointsText();
    }

    [ContextMenu("AddPoint")]
    private void AddPoint()
    {
        _skillManager.AddSkillPointsByType(type, 1);
        UpdateSkillPointsText();
    }

    private void RemovePoint(int amount)
    {
        _skillManager.RemoveSkillPointsByType(type, amount);
        UpdateSkillPointsText();
    }

    public int GetPoints()=> _skillManager.GetSkillPointByType(type);


    private void UpdateSkillPointsText()
    {
        if (_pointsText == null)
            return;
        _pointsText.text = "Points: " + _skillManager.GetSkillPointByType(type);
    }

    private void OnEnable()
    {
        // Loop through the array we found and subscribe to each one
        foreach (var node in _childNodes)
        {
            node.OnSkillUpgradeRemovePoints += RemovePoint;
            //node.OnSkillRemove += RemoveSkillModifiers;
        }
    }

    private void OnDisable()
    {
        // Loop through and unsubscribe
        if (_childNodes != null)
        {
            foreach (var node in _childNodes)
            {
                node.OnSkillUpgradeRemovePoints -= RemovePoint;
                //node.OnSkillRemove -= RemoveSkillModifiers;
            }
        }
    }
}
