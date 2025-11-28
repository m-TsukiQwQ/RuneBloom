using UnityEngine;

public class UIChest : MonoBehaviour
{
    [Header("References")]
    public Transform itemsPanel;
    public GameObject slotPrefab;
    public Canvas mainCanvas;

    private InventoryBase _currentChest;
    public InventoryBase CurrentChest => _currentChest;
    private UIInventorySlot[] _uiSlots;

    private void Awake()
    {
       
        if (mainCanvas == null) mainCanvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
         gameObject.SetActive(false);
    }

    public void Show(InventoryBase chestData)
    {
        _currentChest = chestData;
        gameObject.SetActive(true);

        _currentChest.OnInventoryChanged += UpdateUI;

        InitializeChestSlots();
        UpdateUI();
    }

    public void Hide()
    {
        if (_currentChest != null)
            _currentChest.OnInventoryChanged -= UpdateUI;

        _currentChest = null;
        gameObject.SetActive(false);
    
}

    private void InitializeChestSlots()
    {
        if (_currentChest == null || _currentChest.slots == null) { Debug.LogError("[UIChest] Chest Data is missing or slots not initialized!"); return; }

        foreach (Transform child in itemsPanel)
            Destroy(child.gameObject);

        int size = _currentChest.slots.Length;
        _uiSlots = new UIInventorySlot[size];

        for (int i = 0; i < size; i++)
        {
            GameObject newSlotObj = Instantiate(slotPrefab, itemsPanel);
            newSlotObj.transform.localScale = Vector3.one;

            _uiSlots[i] = newSlotObj.GetComponent<UIInventorySlot>();

            _uiSlots[i].Init(i, _currentChest, mainCanvas);
        }

    }

    private void UpdateUI()
    {
        if (_currentChest == null) return;

        for (int i = 0; i < _currentChest.slots.Length; i++)
        {
            if (i < _uiSlots.Length)
            {
                _uiSlots[i].UpdateSlot(_currentChest.slots[i]);
            }
        }
    }

}
