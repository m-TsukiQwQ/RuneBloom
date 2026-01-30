using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMainCharacteristicDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerHunger _playerHunger;
    [SerializeField] private MainCharacteristicType _characteristicType;
    [SerializeField] private TextMeshProUGUI _displayTextTMP;
    [SerializeField] private GameObject _displayText;

    private void Awake()
    {
        _displayTextTMP.raycastTarget = false;
        _displayText.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _displayText.SetActive(true);
        
        switch (_characteristicType)
        {
            case MainCharacteristicType.Health:
                _displayTextTMP.text = $"{_playerHealth.CurrentHealth} / {_playerHealth.GetMaxHealth()}";
                break;

            case MainCharacteristicType.Hunger:
                _displayTextTMP.text = $"{_playerHunger.CurrentHunger} / {_playerHunger.GetMaxHunger()}";
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _displayText.SetActive(false);
    }

    private void Update()
    {
        if (_displayText.activeSelf)
        {
            _displayText.transform.position = Input.mousePosition;
        }
    }



}

public enum MainCharacteristicType
{
    Health,
    MagicPower,
    Hunger

}
