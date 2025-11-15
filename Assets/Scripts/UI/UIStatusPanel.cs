using System.Collections.Generic;
using UnityEngine;

public class UIStatusPanel : MonoBehaviour
{
    [Header("Logic Reference")]
    [SerializeField] private EntityStatusHandler _playerStatusHandler; 

    [Header("UI Prefab")]
    [SerializeField] private GameObject _statusIconPrefab; 

    [Header("Icons")]
    [SerializeField] private Sprite _chillIcon;
    [SerializeField] private Sprite _burnIcon;
    [SerializeField] private Sprite _poisonIcon;

    // This dictionary tracks the *live* UI icons that are currently on-screen
    // Key: ElementType, Value: The script attached to the instantiated icon prefab
    private Dictionary<ElementType, UIStatusSlot> m_ActiveIcons = new Dictionary<ElementType, UIStatusSlot>();

    void OnEnable()
    {
        // Subscribe to the events
        _playerStatusHandler.OnEffectApplied += HandleEffectApplied;
        _playerStatusHandler.OnEffectRemoved += HandleEffectRemoved;
    }

    void OnDisable()
    {
        // ALWAYS unsubscribe
        _playerStatusHandler.OnEffectApplied -= HandleEffectApplied;
        _playerStatusHandler.OnEffectRemoved -= HandleEffectRemoved;
    }

    private void HandleEffectApplied(ElementType type, int charges)
    {
        if (m_ActiveIcons.TryGetValue(type, out UIStatusSlot iconUI))
        {
            iconUI.UpdateDisplay(GetIconForType(type), charges);
        }
        else
        {
            GameObject iconGO = Instantiate(_statusIconPrefab, transform); // 'transform' is this panel

            UIStatusSlot newIconUI = iconGO.GetComponent<UIStatusSlot>();
            newIconUI.UpdateDisplay(GetIconForType(type), charges);

            m_ActiveIcons[type] = newIconUI;
        }
    }

    private void HandleEffectRemoved(ElementType type)
    {
        if (m_ActiveIcons.TryGetValue(type, out UIStatusSlot iconUI))
        {
            m_ActiveIcons.Remove(type);
            Destroy(iconUI.gameObject);
        }
    }

    private Sprite GetIconForType(ElementType type)
    {
        if (type == ElementType.Ice)
            return _chillIcon;
        if (type == ElementType.Fire) 
            return _burnIcon;
        if (type == ElementType.Poison) 
            return _poisonIcon;


        return null; 
    }
}
