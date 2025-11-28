using TMPro;
using UnityEngine;
using System.Text;

public class UIItemToolTip : UIToolTip
{

    [Header("Configuration")]
    [SerializeField] private TextMeshProUGUI _itemNameTMP;
    [SerializeField] private TextMeshProUGUI _itemDescriptionTMP;
    [SerializeField] private TextMeshProUGUI _itemStatsTMP;
    [SerializeField] private TextMeshProUGUI _itemType;

    [SerializeField] private GameObject _itemDescription;
    [SerializeField] private GameObject _itemStats;

    [Header("Color configuration")]
    [SerializeField] private string _typeHex;
    [SerializeField] private string _statsHex;
    [SerializeField] private string importantInforamtionHex;
    [SerializeField] private Color exampleColor;

    public void ShowItemToolTip(ItemDataSO itemData)
    {
        SetToolTipPosition();
        _itemNameTMP.text = $"{itemData.itemName}";

        _itemStatsTMP.text =  GetStats(itemData);
        _itemStats.gameObject.SetActive(_itemStatsTMP.text.Length == 0 ? false : true);

        _itemDescriptionTMP.text = $"{itemData.description}";
        _itemDescription.gameObject.SetActive(_itemDescriptionTMP.text.Length == 0 ? false : true);

        _itemType.text = GetColoredText($"{itemData.itemDisplayType.ToString()}", _typeHex) ;

    }

    public string GetStats(ItemDataSO itemData)
    {
        StringBuilder sb = new StringBuilder();

        if (itemData is EquipmentDataSO equipmentData)
        {
            foreach (var mod in equipmentData.modifiers)
            {
                sb.AppendLine(GetColoredText($"+ {mod.value} {mod.statType.ToString()}",_statsHex));
            }

        }

        return sb.ToString();
    }

}
