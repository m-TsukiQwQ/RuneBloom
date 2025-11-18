using UnityEngine;

[CreateAssetMenu(menuName = "Game Setup/Skill Data", fileName = "Skill data - ")]
public class SkillDataSo : ScriptableObject
{
    public int cost;
    [Header("Skill description")]
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;
}
