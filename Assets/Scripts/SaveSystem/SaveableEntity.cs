using UnityEngine;
using System;

[ExecuteInEditMode]
public class SaveableEntity : MonoBehaviour
{
    [SerializeField] private string _id = "";

    public string Id => _id;

    [ContextMenu("Generate ID")]
    public void GenerateId()
    {
        _id = Guid.NewGuid().ToString();
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    private void Awake()
    {
        if (string.IsNullOrEmpty(_id))
        {
            GenerateId();
        }
    }

    // --- NEW: CALL THIS FROM YOUR BUILDING SYSTEM ---
    public void RestoreId(string id)
    {
        _id = id;
    }


    private void OnValidate()
    {
        if (string.IsNullOrEmpty(_id))
        {
            GenerateId();
        }
    }
}