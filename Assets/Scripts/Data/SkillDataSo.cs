using UnityEngine;

[CreateAssetMenu(menuName = "Game Setup/Skill Data", fileName = "Skill data - ")]
public class SkillDataSo : ScriptableObject
{
    [Header("Skill description")]
    public string skillName;
    [TextArea]
    public string description;
    public float descriptionNumber;
    public Sprite icon;
    public int maximumLevel;

    public StatType statToUpgrade;
    public float value;
    public bool isMultiplier;
}
