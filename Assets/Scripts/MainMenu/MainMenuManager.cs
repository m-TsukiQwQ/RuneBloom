using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _mainMenuPanel;


    public void NewGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void EnterSettings()
    {
        _settingsPanel.SetActive(true);
        _mainMenuPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    

}
