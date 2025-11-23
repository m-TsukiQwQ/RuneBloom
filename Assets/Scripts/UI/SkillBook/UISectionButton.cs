using UnityEngine;
using UnityEngine.UI;

public class UISectionButton : MonoBehaviour
{
    [SerializeField] private GameObject _page;
    [SerializeField] private Sprite _selectedIcon;
    [SerializeField] private Sprite _unselectedIcon;
    private Image _image;


    private void Awake()
    {
        _image = GetComponent<Image>();

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
    }


}
