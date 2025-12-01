using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICraftingSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    private UIManager _ui;

    [Header("Appereance configuration")]
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _unselectedSprite;
    [SerializeField] private Image _bacground;
    private string _lockedColorHex = "#8E8E8E";

    [SerializeField] private CraftingRecipeSO _recipeData;

    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _quantityText;

    public UICraft uiCraft;

    public void Init(CraftingRecipeSO recipe, UICraft parentUI)
    {
        _recipeData = recipe;
        uiCraft = parentUI;

        if (_recipeData != null && _recipeData.craftItem != null)
        {
            _itemIcon.sprite = _recipeData.craftItem.itemIcon; 
            _itemIcon.enabled = true;

            if (_recipeData.quantity > 1)
            {
                _quantityText.text = _recipeData.quantity.ToString();
                _quantityText.enabled = true;
            }
            else
            {
                _quantityText.enabled = false;
            }
        }
    }

    private void Awake()
    {
        _ui = GetComponentInParent<UIManager>();
    }
    public void UpdateSlot()
    {
        if (IsEnoughIngredient())
            UpdateIconColor(Color.white);
        else
            UpdateIconColor(GetColorByHex(_lockedColorHex));

    }

    public bool IsEnoughIngredient()
    {

        foreach (var ingredient in _recipeData.ingredients)
        {
            if (uiCraft.playerInventory.GetTotalAmount(ingredient.ItemData) < ingredient.amount)
                return false;
        }

        return true;
    }

    private void UpdateIconColor(Color color)
    {
        if (_itemIcon == null)
            return;
        _bacground.color = color;
        _itemIcon.color = color;
    }
    private Color GetColorByHex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (_recipeData != null)
        {
            
            uiCraft.OnCraftRequested(_recipeData);
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.craftItemToolTip.ShowCraftItemToolTip(_recipeData, this);
        _ui.ShowCraftItemToolTip(true);
        _bacground.sprite = _selectedSprite;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.ShowCraftItemToolTip(false);
        _bacground.sprite = _unselectedSprite;

    }
}
