using System.Collections.Generic;
using UnityEngine;

public class UIStatusPanel : MonoBehaviour
{
    [Header("Logic Reference")]
    [SerializeField] private EntityStatusHandler playerStatusHandler; // Drag the Player here

    [Header("UI Prefab")]
    [SerializeField] private GameObject statusIconPrefab; // Drag your StatusEffectIcon_Prefab here

    [Header("Icon Mapping")]
    // We need a way to map the ElementType enum to an actual Sprite
    // This is a simple way to do it.
    [SerializeField] private Sprite _chillIcon;
    [SerializeField] private Sprite _burnIcon; // Add others for the future

    // This dictionary tracks the *live* UI icons that are currently on-screen
    // Key: ElementType, Value: The script attached to the instantiated icon prefab
    private Dictionary<ElementType, UIStatusSlot> m_ActiveIcons = new Dictionary<ElementType, UIStatusSlot>();

    void OnEnable()
    {
        // Subscribe to the events
        playerStatusHandler.OnEffectApplied += HandleEffectApplied;
        playerStatusHandler.OnEffectRemoved += HandleEffectRemoved;
    }

    void OnDisable()
    {
        // ALWAYS unsubscribe
        playerStatusHandler.OnEffectApplied -= HandleEffectApplied;
        playerStatusHandler.OnEffectRemoved -= HandleEffectRemoved;
    }

    private void HandleEffectApplied(ElementType type, int charges)
    {
        // Check if we *already* have an icon for this effect
        if (m_ActiveIcons.TryGetValue(type, out UIStatusSlot iconUI))
        {
            // --- This is an UPDATE ---
            // We found an existing icon, so just update its charges
            iconUI.UpdateDisplay(GetIconForType(type), charges);
        }
        else
        {
            // --- This is a NEW effect ---
            // 1. Create a new UI icon from the prefab
            GameObject iconGO = Instantiate(statusIconPrefab, transform); // 'transform' is this panel

            // 2. Get its helper script
            UIStatusSlot newIconUI = iconGO.GetComponent<UIStatusSlot>();

            // 3. Update its display
            newIconUI.UpdateDisplay(GetIconForType(type), charges);

            // 4. Store it in our dictionary so we can find it later
            m_ActiveIcons[type] = newIconUI;
        }
    }

    private void HandleEffectRemoved(ElementType type)
    {
        // 1. Find the UI icon for this effect
        if (m_ActiveIcons.TryGetValue(type, out UIStatusSlot iconUI))
        {
            // 2. Remove it from the dictionary
            m_ActiveIcons.Remove(type);

            // 3. Destroy the UI GameObject
            Destroy(iconUI.gameObject);
        }
    }

    // A helper function to get the correct sprite
    private Sprite GetIconForType(ElementType type)
    {
        if (type == ElementType.Ice)
            return _chillIcon;
        if (type == ElementType.Fire) // Example for the future
            return _burnIcon;

        // Add other types here

        return null; // Default/error case
    }
}
