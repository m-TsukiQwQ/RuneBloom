using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 mouseDirection {  get; private set; }
    private Camera _mainCamera;
    private Vector2 screen;

    public static InputHandler Instance { get; private set; }
    [Tooltip("Set this to the layer(s) your INTERACTABLE UI (buttons, etc.) is on. Clicks on other UI layers will be ignored.")]
    [SerializeField] private LayerMask interactableUILayerMask;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate InputHandler instance found. Destroying self.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        screen = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        _mainCamera = Camera.main;
    }


    public void OnClick(InputAction.CallbackContext context)
    {


        if (!context.started) return;

        if (IsPointerOverUI())
            return;

        CalculateMousePosition();

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if(!rayHit.collider) return;

    }
    


    private Vector2 CalculateMousePosition()
    {
        float x = (Input.mousePosition.x - (Screen.width / 2.0f)) / (Screen.width / 2.0f);
        float y = (Input.mousePosition.y - (Screen.height / 2.0f)) / (Screen.height / 2.0f);

        // Clamping ensures the value never goes beyond -1 or 1, even if the mouse
        // leaves the game window (in some setups).
        x = Mathf.Clamp(x, -1.0f, 1.0f);
        y = Mathf.Clamp(y, -1.0f, 1.0f);

        mouseDirection = new Vector2(x, y);
        return mouseDirection;
    }

    public static bool IsPointerOverUI() // It's public and static
    {
        // Create a PointerEventData object for the current mouse position
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();

        // Create a list to store the raycast results
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast against all UI elements
        EventSystem.current.RaycastAll(eventData, results);

        // Check if our singleton Instance is set and the mask is assigned
        if (Instance == null || Instance.interactableUILayerMask.value == 0)
        {
            if (Instance != null)
            {
                Debug.LogWarning("InputHandler 'Interactable UI Layer Mask' is not set in the Inspector. Defaulting to block all UI clicks.", Instance);
            }

            // Default to original behavior (block all UI) if not configured
            return results.Count > 0;
        }

        // Loop through all the UI elements the raycast hit
        for (int i = 0; i < results.Count; i++)
        {
            // Check if the layer of the hit object is IN our interactable mask
            // (1 << layer) creates a bitmask for that single layer
            // We then use the & operator to see if it's part of our mask
            if (((1 << results[i].gameObject.layer) & Instance.interactableUILayerMask) != 0)
            {
                // We hit a "real" UI element, so return true
                return true;
            }
        }

        // We looped through everything and only hit "ignorable" UI (or nothing)
        return false;
    }
}
