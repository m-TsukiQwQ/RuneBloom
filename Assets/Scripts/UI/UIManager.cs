using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI logic")]
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _skillBook;

    [Header("Overlay logic")]
    [SerializeField] private GameObject _freezeOverlay;

    [HideInInspector] public UISkillToolTip skillToolTip;

    [SerializeField] private List<GameObject> _skillPages;
    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UISkillToolTip>(true);
    }
    private void Start()
    {
        ShowToolTip(false);
    }

    public void ToggleInventory()
    {
        if (_skillBook.activeSelf)
            ToggleSkillBook();
        _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
    }
    public void ToggleSkillBook()
    {
        if(_inventoryPanel.activeSelf)
            ToggleInventory();
        _skillBook.SetActive(!_skillBook.activeSelf);
    }

    public void ShowCloseFreezeOverlay()
    {
        _freezeOverlay.SetActive(!_freezeOverlay.activeSelf);
    }


    public virtual void ShowToolTip(bool show)
    {
        skillToolTip.gameObject.SetActive(show);
        

    }

    public void HideAllPages()
    {
        foreach(var page in _skillPages)
        {
            page.gameObject.SetActive(false);
        }
    }


}
