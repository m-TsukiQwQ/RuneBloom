using UnityEditor;
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
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        string guid = AssetDatabase.AssetPathToGUID(path);

        if (!string.IsNullOrEmpty(guid) && itemID != guid)
        {
            itemID = guid;

            EditorUtility.SetDirty(this);
        }
#endif
    }


    [ContextMenu("Regenerate ID")]
    public void RegenerateID()
    {
#if UNITY_EDITOR
        // Force reset to the file's GUID
        string path = AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path);
        EditorUtility.SetDirty(this);
        Debug.Log($"Reset ID to File GUID: {itemID}");
#endif
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
