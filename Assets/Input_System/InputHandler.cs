using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 mouseDirection {  get; private set; }
    private Camera _mainCamera;
    private Vector2 screen;

    private void Awake()
    {
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

    public static bool IsPointerOverUI()
    {
        // Create a PointerEventData object for the current mouse position
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();

        // Create a list to store the raycast results
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast against all UI elements
        EventSystem.current.RaycastAll(eventData, results);

        // If the list has one or more results, the mouse is over UI
        return results.Count > 0;
    }
}
