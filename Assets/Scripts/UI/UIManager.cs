using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI logic")]
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _skillBookPanel;
    [SerializeField] private GameObject _chestPanel;

    [SerializeField] private GameObject[] _panels;

    [Header("Overlay logic")]
    [SerializeField] private GameObject _freezeOverlay;

    [HideInInspector] public UISkillToolTip skillToolTip;
    [HideInInspector] public UIItemToolTip itemToolTip;

    [SerializeField] private List<GameObject> _skillPages;
    [SerializeField] private GameObject _defaultPage;

    private UISectionButton _currentActiveButton;

    [SerializeField ] private GameObject _darkBG;

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UISkillToolTip>(true);
        itemToolTip = GetComponentInChildren<UIItemToolTip>(true);

        _panels = new GameObject[3];
        _panels[0] = _inventoryPanel;
        _panels[1] = _chestPanel;
        _panels[2] = _skillBookPanel;

    }
    private void Update()
    {
        
    }
    private void Start()
    {
        ShowSkillToolTip(false);
        ShowItemToolTip(false);
        _inventoryPanel.SetActive(false);
    }

    public void ToggleInventory()
    {
        foreach (var panel in _panels)
        {
            if (panel != _inventoryPanel)
                panel.SetActive(false);
        }
        _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
        _darkBG.SetActive(_inventoryPanel.activeSelf);
        

    }
    public void HideAllUI()
    {
        _inventoryPanel.SetActive(false);
        _skillBookPanel.SetActive(false);
        _chestPanel.SetActive(false);
    }

    public void ToggleInventoryPanel()
    {

    }

    public void ToggleSkillBook()
    {
        foreach (var panel in _panels)
        {
            if (panel != _skillBookPanel)
                panel.SetActive(false);
        }
        _skillBookPanel.SetActive(!_skillBookPanel.activeSelf);
        _darkBG.SetActive(_skillBookPanel.activeSelf);
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
