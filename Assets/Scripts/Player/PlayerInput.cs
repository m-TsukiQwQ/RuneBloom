using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private UIManager _ui;
    private PlayerInputSystem _input;
    private InventoryBase _playerInventory;


    private void Awake()
    {
        _input = new PlayerInputSystem();
        _ui = FindFirstObjectByType<UIManager>();
        _playerInventory = FindFirstObjectByType<InventoryBase>();
    }

    private void OnEnable()
    {
        _input.Enable();

        _input.UI.ToggleInventory.performed += ctx => _ui.ToggleInventory();
        _input.UI.ToggleSkillBook.performed += ctx => _ui.ToggleSkillBook();
        _input.UI.SortInventory.performed += ctx => _playerInventory.SortInventory();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
