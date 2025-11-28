using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private UIManager _ui;
    private PlayerInputSystem _input;
    private PlayerInventory _playerInventory;
    [SerializeField] private PlayerInteraction _playerInteraction;


    private void Awake()
    {
        _input = new PlayerInputSystem();
        _ui = FindFirstObjectByType<UIManager>();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerInteraction = GetComponent<PlayerInteraction>();
    }

    private void OnEnable()
    {
        _input.Enable();

        _input.UI.ToggleInventory.performed += ctx => _ui.ToggleInventory();
        _input.UI.ToggleSkillBook.performed += ctx => _ui.ToggleSkillBook();
        _input.UI.SortInventory.performed += ctx => _playerInventory.SortPlayersInventory();
        _input.Player.Interact.performed += ctx => _playerInteraction.Interact(); 
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
