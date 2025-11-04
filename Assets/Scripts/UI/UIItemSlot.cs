using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public Inventory_Item itemInSlot { get; private set; }


    [Header("UI Slot Setup")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemStackSize;



    public void UpdateSlot(Inventory_Item item)
    {
        itemInSlot = item;
        if (itemInSlot == null)
        {
            _itemStackSize.text = "";
            _itemIcon.sprite = null;
            _itemIcon.color = Color.clear;
            return;
        }
        Color color = Color.white; 
        _itemIcon.color = color;
        _itemIcon.sprite = itemInSlot.itemData.itemIcon;
        _itemStackSize.text = item.stackSize > 1 ? item.stackSize.ToString() : "";

    }
}
