using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI logic")]
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _spellBook;

    [Header("Overlay logic")]
    [SerializeField] private GameObject _freezeOverlay;

    [HideInInspector] public UISkillToolTip skillToolTip;

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
        if (_spellBook.activeSelf)
            ToggleSpellBook();
        _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
    }
    public void ToggleSpellBook()
    {
        if(_inventoryPanel.activeSelf)
            ToggleInventory();
        _spellBook.SetActive(!_spellBook.activeSelf);
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
