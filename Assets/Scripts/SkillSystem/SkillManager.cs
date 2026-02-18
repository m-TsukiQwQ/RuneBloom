using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private Dictionary<UISkillBookPageType, int> _skillPoints = new Dictionary<UISkillBookPageType, int>();


    private EntityStats _statsToModify;

    private void Awake()
    {
        _statsToModify = GetComponent<EntityStats>();
        foreach (UISkillBookPageType type in System.Enum.GetValues(typeof(UISkillBookPageType)))
        {
            _skillPoints[type] = 0;
        }
    }

    private void AddSkillModifiers(SkillDataSo skill)
    {
        if (skill == null || skill.statToUpgrade == StatType.None) return;

        if (skill.isMultiplier)

            _statsToModify.GetStatByType(skill.statToUpgrade).AddMultiplier(skill.value, skill.skillName);
        else
            _statsToModify.GetStatByType(skill.statToUpgrade).AddModifier(skill.value, skill.skillName);
    }

    private void RemoveSkillModifiers(SkillDataSo skill)
    {

    }

    void OnEnable()
    {

        UITreeNode.OnSkillUpgrade += AddSkillModifiers;
        UITreeNode.OnSkillRemove += RemoveSkillModifiers;
    }

    void OnDisable()
    {

        UITreeNode.OnSkillUpgrade -= AddSkillModifiers;
        UITreeNode.OnSkillRemove -= RemoveSkillModifiers;
    }

    public int GetSkillPointByType(UISkillBookPageType skill)
    {
        return _skillPoints.ContainsKey(skill) ? _skillPoints[skill] : 0;
    }

    public void ModifySkillPoints(UISkillBookPageType skill, int amount)
    {
        if (_skillPoints.ContainsKey(skill))
        {
            _skillPoints[skill] += amount;
        }
        else
        {
            Debug.Log($"Skill {skill} not initialized.");
        }
    }

    public void AddSkillPointsByType(UISkillBookPageType skill, int amount) => ModifySkillPoints(skill, amount);
    public void RemoveSkillPointsByType(UISkillBookPageType skill, int amount) => ModifySkillPoints(skill, -amount);


}

