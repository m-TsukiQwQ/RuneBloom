using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{

    [SerializeField] GameObject _slotPrefab;

    [Header("Prefabs")]
    [SerializeField] private UIInventorySlot[] _allSlots;
    [SerializeField] private int _gridSlotCount = 24;
    [SerializeField] private int _inventoryStartIndex;

    [SerializeField] private int _toolbarSize = 8;
    [SerializeField] private GameObject _toolbarPanel;

    [Header("References")]
    public InventoryBase inventory;
    public Transform itemsParent;
    public Canvas mainCanvas;


    [Header("Equipment Slots")]
    [SerializeField] private UIEquipmentSlot hatSlot;
    [SerializeField] private UIEquipmentSlot shirtSlot;
    [SerializeField] private UIEquipmentSlot pantsSlot;
    [SerializeField] private UIEquipmentSlot necklaceSlot;
    [SerializeField] private UIEquipmentSlot ringSlot;
    [SerializeField] private UIEquipmentSlot bootsSlot;

    [SerializeField] private GameObject _equipmentPanel;

    public Image darkBGButton;

    private int selectedSlot = -1;

    public void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
            _allSlots[selectedSlot].Deselect();

        _allSlots[newValue].Select();
        selectedSlot = newValue;
    }


    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 9)
            {
                ChangeSelectedSlot(number - 1);
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            int newValue = selectedSlot - (int)(scroll / Mathf.Abs(scroll));
            if (newValue < 0)
            {
                newValue = _toolbarSize - 1;
            }
            else if (newValue >= _toolbarSize)
            {
                newValue = 0;
            }
            ChangeSelectedSlot(newValue);
        }

    }
    private void Awake()
    {

        if (inventory == null)
            inventory = FindFirstObjectByType<PlayerInventory>();
        if (mainCanvas == null)
            mainCanvas = GetComponentInParent<Canvas>();

        inventory.OnInventoryChanged += UpdateUI;




        InitializeInventory();

        ChangeSelectedSlot(0);

    }

    private void Start()
    {
        UpdateUI();
    }

    private void InitializeInventory()
    {
        int totalSize = _gridSlotCount + _toolbarSize;

        if (hatSlot != null) totalSize++;
        if (shirtSlot != null) totalSize++;
        if (pantsSlot != null) totalSize++;
        if (necklaceSlot != null) totalSize++;
        if (ringSlot != null) totalSize++;
        if (bootsSlot != null) totalSize++;

        _allSlots = new UIInventorySlot[totalSize];

        int index = 0;
        for (index = 0; index < _toolbarSize; index++)
        {
            GameObject newSlotObj = Instantiate(_slotPrefab);
            newSlotObj.transform.SetParent(_toolbarPanel.transform, false);
            _allSlots[index] = newSlotObj.GetComponent<UIInventorySlot>();
        }

        for (int i = index; index < _gridSlotCount + _toolbarSize; index++)
        {
            GameObject newSlotObj = Instantiate(_slotPrefab);
            newSlotObj.transform.SetParent(itemsParent, false);

            // Assign directly to the array index
            _allSlots[index] = newSlotObj.GetComponent<UIInventorySlot>();
        }

        int currentIndex = index;


        if (hatSlot != null)
        {
            _allSlots[currentIndex] = hatSlot;
            currentIndex++;
        }
        if (hatSlot != null)
        {
            _allSlots[currentIndex] = shirtSlot;
            currentIndex++;
        }
        if (hatSlot != null)
        {
            _allSlots[currentIndex] = pantsSlot;
            currentIndex++;
        }
        if (hatSlot != null)
        {
            _allSlots[currentIndex] = necklaceSlot;
            currentIndex++;
        }
        if (hatSlot != null)
        {
            _allSlots[currentIndex] = ringSlot;
            currentIndex++;
        }
        if (hatSlot != null)
        {
            _allSlots[currentIndex] = bootsSlot;
            currentIndex++;
        }

        // Initialize IDs for everyone
        for (int i = 0; i < _allSlots.Length; i++)
        {
            _allSlots[i].Init(i, inventory, mainCanvas);
        }
        UpdateUI();
    }


    private void UpdateUI()
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (i < _allSlots.Length)
            {
                _allSlots[i].UpdateSlot(inventory.slots[i]);
            }
        }


    }

    public void HandleSwap(int indexA, int indexB)
    {
        inventory.SwapItems(indexA, indexB);
    }

    public void OnQuickEquip(int sourceIndex, int caseOfClcik)
    {
        if (_equipmentPanel.activeSelf == false)
            return;

        bool isBagSlot = sourceIndex < _gridSlotCount + _toolbarSize;

        if (isBagSlot)
        {
            ItemDataSO itemToEquip = inventory.slots[sourceIndex].itemData;
            if (itemToEquip == null)
                return;
            int targetIndex = -1;

            if (hatSlot != null && hatSlot.CanAccept(itemToEquip))
            {
                targetIndex = hatSlot.SlotIndex;
            }
            if (shirtSlot != null && shirtSlot.CanAccept(itemToEquip))
            {
                targetIndex = shirtSlot.SlotIndex;
            }
            if (pantsSlot != null && pantsSlot.CanAccept(itemToEquip))
            {
                targetIndex = pantsSlot.SlotIndex;
            }
            if (necklaceSlot != null && necklaceSlot.CanAccept(itemToEquip))
            {
                targetIndex = necklaceSlot.SlotIndex;
            }
            if (ringSlot != null && ringSlot.CanAccept(itemToEquip))
            {
                targetIndex = ringSlot.SlotIndex;
            }
            if (bootsSlot != null && bootsSlot.CanAccept(itemToEquip))
            {
                targetIndex = bootsSlot.SlotIndex;
            }

            if (targetIndex != -1)
            {
                inventory.SwapItems(sourceIndex, targetIndex);
            }
        }
        else
        {
            for (int i = _inventoryStartIndex; i < _gridSlotCount + _toolbarSize; i++)
            {
                if (!inventory.slots[i].HasItem)
                {
                    inventory.SwapItems(sourceIndex, i);
                    return;
                }
            }

            for (int i = 0; i < _toolbarSize; i++)
            {
                if (!inventory.slots[i].HasItem)
                {
                    inventory.SwapItems(sourceIndex, i);
                    return;
                }
            }



        }
    }

    private void OnDisable()
    {
        UpdateUI();
    }


}
