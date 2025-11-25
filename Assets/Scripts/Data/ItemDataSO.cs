using UnityEngine;

[CreateAssetMenu(menuName = "Game Setup/Item Data/Material item", fileName = "Material data - ")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemDisplayType;
    public EquipmentType equipmentType;
    [TextArea] public string description;
    public int maxStackSize = 1;
}

public enum EquipmentType
{
    None,
    Hat,
    Chestplate,
    Pants,
    Boots,
    Ring,
    Necklace

}
