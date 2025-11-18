using TMPro;
using UnityEngine;

public class UISkillToolTip : UIToolTip
{
    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI _skillDescription;
    [SerializeField] private TextMeshProUGUI _skillRequirements;

    protected override void Update()
    {
        int nameLength = _skillName.text.Length;
        int descriptionLength = _skillDescription.text.Length;

        _layoutElement.enabled = (nameLength > _characterWrapLimit || descriptionLength > _characterWrapLimit) ? true : false;

        base.Update();
    }
    public void ShowSkillToolTip(SkillDataSo skillData)
    {
        _skillName.text = skillData.skillName;
        _skillDescription.text = skillData.description;
        _skillRequirements.text = $"Requirements: \n - {skillData.cost.ToString()} skill points";
    }
}
