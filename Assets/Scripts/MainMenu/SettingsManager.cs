using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _mainMenuPanel;


    private UISectionButton _currentActiveButton;
    [SerializeField] private GameObject _defaultPage;
    [SerializeField] private List<GameObject> _settingsPages;

    [SerializeField] private UISectionButton _defaultButton;

    private void Start()
    {
        HideAllPages();
        ForceButton();
    }

    public void ExitSettings()
    {
        _settingsPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
    }


    public void HideAllPages()
    {
        foreach (var page in _settingsPages)
        {
            page.gameObject.SetActive(false);
        }
    }

    public void ForceButton()
    {
        if( _currentActiveButton != null )
        {
            _currentActiveButton.SetState(false);
        }
        _currentActiveButton = _defaultButton;
        _currentActiveButton.SetState(true);


    }

    public void OnSectionButtonClicked()
    {

        GameObject clickedObj = EventSystem.current.currentSelectedGameObject;
        if (clickedObj == null)
        {
            Debug.Log("clickedObj == null");
            return;
        }

        UISectionButton clickedButton = clickedObj.GetComponent<UISectionButton>();
        if (clickedButton == null)
        {
            Debug.Log("Button == null");
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
