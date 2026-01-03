using UnityEngine;

[CreateAssetMenu(menuName = "Game Setup/Item Data/Material item", fileName = "Material data - ")]
public class ItemDataSO : ScriptableObject
{

    public string itemID;

    [Header("Visuals & Data")]
    public string itemName;
    public Sprite itemIcon;
    public Sprite inHandIcon;
    [TextArea] public string description;

    [Header("Mechanics")]
    public ItemType itemDisplayType;
    public EquipmentType equipmentType;
    public int maxStackSize = 1;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(itemID))
        {
            itemID = System.Guid.NewGuid().ToString();
        }
    }
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
