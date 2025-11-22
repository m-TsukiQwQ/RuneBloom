using System.Text;
using TMPro;
using UnityEngine;

public class UISkillToolTip : UIToolTip
{
    [Header("Configuration")]
    [SerializeField] private TextMeshProUGUI _skillNameTMP;
    [SerializeField] private TextMeshProUGUI _skillDescriptionTMP;
    [SerializeField] private TextMeshProUGUI _skillRequirementsTMP;

    [SerializeField] private GameObject _skillDescription;
    [SerializeField] private GameObject _skillRequirements;

    [Header("Color configuration")]
    [SerializeField] private string metConditionHex;
    [SerializeField] private string notMetConditionHex;
    [SerializeField] private string importantInforamtionHex;
    [SerializeField] private Color exampleColor;

    protected override void Update()
    {
        int nameLength = _skillNameTMP.text.Length;
        int descriptionLength = _skillDescriptionTMP.text.Length;

        _layoutElement.enabled = (nameLength > _characterWrapLimit || descriptionLength > _characterWrapLimit) ? true : false;

        base.Update();
    }
    public void ShowSkillToolTip(SkillDataSo skillData, UITreeNode node)
    {
        SetToolTipPosition();
        _skillNameTMP.text = $"{skillData.skillName} {node.currentNodeLevel}/{skillData.maximumLevel}";

        _skillDescriptionTMP.text = GetDescription(skillData, node.currentNodeLevel);
        _skillDescription.gameObject.SetActive(_skillDescriptionTMP.text.Length == 0 ? false : true);

        _skillRequirementsTMP.text = GetRequirements(node.neededNodes, node.conflictNodes, skillData.cost, node.currentNodeLevel, skillData.maximumLevel, node.GetCurrentPoints());
        _skillRequirements.gameObject.SetActive(_skillRequirementsTMP.text.Length == 0 ? false : true);
    }

    public string GetDescription(SkillDataSo skill, int currentNodeLevel)
    {
        StringBuilder sb = new StringBuilder();
        if (currentNodeLevel < skill.maximumLevel)
        {
            if (currentNodeLevel > 0)
            {
                sb.AppendLine("Current rank: ");
                sb.AppendLine($"+{skill.descriptionNumber * currentNodeLevel} {skill.description}");
                sb.AppendLine();
            }

            sb.AppendLine("Next rank: ");
            sb.AppendLine($"{GetColoredText($"+{skill.descriptionNumber * (currentNodeLevel + 1)} {skill.description}", importantInforamtionHex)}");

        }
        else if (currentNodeLevel >= skill.maximumLevel)
        {
            sb.AppendLine("Current rank: ");
            sb.AppendLine(GetColoredText($"+{skill.descriptionNumber * currentNodeLevel} {skill.description}", importantInforamtionHex));
        }

        return sb.ToString();
    }

    public string GetRequirements(UITreeNode[] neededNodes, UITreeNode[] conflictNodes, int cost, int currentNodeLevel, int maximumLevel, int currentPoints)
    {
        StringBuilder sb = new StringBuilder();
        if (currentNodeLevel < maximumLevel)
        {
            if (currentNodeLevel < 1)
            {
                if (neededNodes.Length != 0 || conflictNodes.Length != 0)
                {
                    if (neededNodes.Length != 0)
                    {
                        sb.AppendLine("Requiremnets:");
                        foreach (var node in neededNodes)
                        {
                            string nodeColor = node.isFullyUnlocked ? metConditionHex : notMetConditionHex;
                            sb.AppendLine(GetColoredText($"- {node._skillData.skillName}", nodeColor));
                            nodeColor = currentPoints >= cost ? metConditionHex : notMetConditionHex;
                            sb.AppendLine(GetColoredText($"- {cost} points", nodeColor));
                        }

                    }

                    if (conflictNodes.Length != 0)
                    {
                        if (neededNodes.Length != 0)
                            sb.AppendLine();
                        sb.AppendLine(GetColoredText("Locks out:", importantInforamtionHex));
                        foreach (var node in conflictNodes)
                        {
                            sb.AppendLine(GetColoredText($"- {node._skillData.skillName}", importantInforamtionHex));
                        }

                    }
                }
                else
                {
                    sb.AppendLine("Requiremnets:");
                    string color = currentPoints >= cost ? metConditionHex : notMetConditionHex;
                    sb.AppendLine(GetColoredText($"- {cost} point(s)", color));
                }
            }
            else
            {
                sb.AppendLine("Requiremnets:");
                string color = currentPoints >= cost ? metConditionHex : notMetConditionHex;
                sb.AppendLine(GetColoredText($"- {cost} point(s)", color));
            }

        }

        return sb.ToString();
    }


}
