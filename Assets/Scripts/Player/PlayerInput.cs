using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private UIManager _ui;
    private PlayerInputSystem _input;

    private void Awake()
    {
        _input = new PlayerInputSystem();
        _ui = FindFirstObjectByType<UIManager>();
    }

    private void OnEnable()
    {
        _input.Enable();

        _input.UI.ToggleInventory.performed += ctx => _ui.ToggleInventory();
        _input.UI.ToggleSkillBook.performed += ctx => _ui.ToggleSkillBook();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
