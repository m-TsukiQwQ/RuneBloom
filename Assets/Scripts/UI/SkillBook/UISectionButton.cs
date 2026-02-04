using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class UISectionButton : MonoBehaviour
{
    [SerializeField] private GameObject _page;
    [SerializeField] private Sprite _selectedIcon;
    [SerializeField] private Sprite _unselectedIcon;
    private Image _image;

    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _unselectedColor;
    private TextMeshProUGUI _text;


    private void Awake()
    {
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();

        UpdateVisuals(false);
    }
   

    public void SetState(bool isActive)
    {
        if (_page != null)
        {
            _page.SetActive(isActive);
        }
        UpdateVisuals(isActive);
    }

    private void UpdateVisuals(bool isActive)
    {
        if (_image != null)
        {
            _image.sprite = isActive ? _selectedIcon : _unselectedIcon;
        }

        if (_text != null)
        {
            Color color = isActive ? _selectedColor : _unselectedColor;

            _text.color = color;
        }
    }


}
