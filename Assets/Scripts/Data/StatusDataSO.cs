using UnityEngine;

[CreateAssetMenu(menuName = "Game Setup/Item Data/Material item", fileName = "Material data - ")]
public class StatusDataSO : ScriptableObject
{
    public Sprite statusIcon;
    public int charges = 1;
}
