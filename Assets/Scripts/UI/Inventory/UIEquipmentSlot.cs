using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentSlot : UIInventorySlot
{
    [Header("Equipment Rules")]
    public EquipmentType allowedType;
    [SerializeField] private Sprite _originalBG;
    [SerializeField] private Sprite _blankBG;
    private Image _bG;

    private void Awake()
    {

        _bG = GetComponent<Image>();
    }
    public override bool CanAccept(ItemDataSO item)
    {

        if (item == null) return true; // Empty is fine

        // Only return true if the types match
        return item.equipmentType == allowedType;
    }

    public override void UpdateSlot(InventorySlot slot)
    {
        base.UpdateSlot(slot);
        if (slot != null && slot.HasItem)
        {
            _bG.sprite = _blankBG;

        }
        else
        {
            _bG.sprite = _originalBG;
        }
    }
}
