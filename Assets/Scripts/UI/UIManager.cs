using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryPanel;

    public void ShowCloseInventory()
    {
        _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
    }
}
