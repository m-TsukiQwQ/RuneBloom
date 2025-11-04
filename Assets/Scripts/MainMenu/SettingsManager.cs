using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _mainMenuPanel;
    public void ExitSettings()
    {
        _settingsPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
    }
}
