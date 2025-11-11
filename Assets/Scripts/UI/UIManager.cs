using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI logic")]
    [SerializeField] private GameObject _inventoryPanel;


    [Header("Overlay logic")]
    [SerializeField] private GameObject _freezeOverlay;
    public void ShowCloseInventory()
    {
        _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
    }

    public void ShowCloseFreezeOverlay()
    {
        _freezeOverlay.SetActive(!_freezeOverlay.activeSelf);
    }


}
