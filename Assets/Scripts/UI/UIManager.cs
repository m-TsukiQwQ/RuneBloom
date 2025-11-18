using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI logic")]
    [SerializeField] private GameObject _inventoryPanel;


    [Header("Overlay logic")]
    [SerializeField] private GameObject _freezeOverlay;

    public UISkillToolTip skillToolTip;

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UISkillToolTip>();
    }
    private void Start()
    {
        ShowToolTip(false);
    }

    public void ShowCloseInventory()
    {
        _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
    }

    public void ShowCloseFreezeOverlay()
    {
        _freezeOverlay.SetActive(!_freezeOverlay.activeSelf);
    }


    public virtual void ShowToolTip(bool show)
    {
        skillToolTip.gameObject.SetActive(show);
        

    }


}
