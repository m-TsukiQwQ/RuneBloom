using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI logic")]
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _skillBook;

    [Header("Overlay logic")]
    [SerializeField] private GameObject _freezeOverlay;

    [HideInInspector] public UISkillToolTip skillToolTip;
    [HideInInspector] public UIItemToolTip itemToolTip;

    [SerializeField] private List<GameObject> _skillPages;
    [SerializeField] private GameObject _defaultPage;

    private UISectionButton _currentActiveButton;

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UISkillToolTip>(true);
        itemToolTip = GetComponentInChildren<UIItemToolTip>(true);


    }
    private void Update()
    {
        
    }
    private void Start()
    {
        ShowSkillToolTip(false);
        ShowItemToolTip(false);
        ToggleInventory();
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


    public virtual void ShowSkillToolTip(bool show)
    {
        skillToolTip.gameObject.SetActive(show);
        

    }

    public virtual void ShowItemToolTip(bool show)
    {
        itemToolTip.gameObject.SetActive(show);
        

    }

    public void HideAllPages()
    {
        foreach(var page in _skillPages)
        {
            page.gameObject.SetActive(false);
        }
    }


    public void OnSectionButtonClicked()
    {
        
        GameObject clickedObj = EventSystem.current.currentSelectedGameObject;
        if (clickedObj == null) return;

        UISectionButton clickedButton = clickedObj.GetComponent<UISectionButton>();
        if (clickedButton == null) return;

        
        if (_currentActiveButton == clickedButton)
        {
            _currentActiveButton.SetState(false);
            _currentActiveButton = null;
            _defaultPage.SetActive(true);
            return;
        }

        if (_currentActiveButton != null)
        {
            _currentActiveButton.SetState(false);
        }

        
        clickedButton.SetState(true);

        
        _currentActiveButton = clickedButton;
    }



}
