using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private EntityStats _statsToModify;

    private void Awake()
    {
        _statsToModify = GetComponent<EntityStats>();
    }

    private void AddSkillModifiers(SkillDataSo skill)
    {
        if(skill == null || skill.statToUpgrade == StatType.None) return;
        _statsToModify.GetStatByType(skill.statToUpgrade).AddModifier(skill.value,skill.skillName);
    }

    private void RemoveSkillModifiers(SkillDataSo skill)
    {

    }

    void OnEnable()
    {
        // Subscribe to the events
        UITreeNode.OnSkillUpgrade += AddSkillModifiers;
        UITreeNode.OnSkillRemove += RemoveSkillModifiers;
    }

    void OnDisable()
    {
        // ALWAYS unsubscribe
        UITreeNode.OnSkillUpgrade -= AddSkillModifiers;
        UITreeNode.OnSkillRemove -= RemoveSkillModifiers;
    }
}
