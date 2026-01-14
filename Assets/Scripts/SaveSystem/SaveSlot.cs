using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string _profileId = "";


    [Header("Data Fields")]
    [SerializeField] private TextMeshProUGUI _dayCountText;
    [SerializeField] private TextMeshProUGUI _lastPlayedText; // Optional: To show date

    private Button _button;

    public string GetProfileId() => _profileId;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    public void Initialize(string profileId, GameData data)
    {
        _profileId = profileId;
        SetData(data);
    }

    public void SetData(GameData data)
    {
        
            if (_dayCountText) _dayCountText.text = $"Day {data.dayCount}";

    }

    public void SetInteractable(bool interactable)
    {
        if (_button) _button.interactable = interactable;
    }
}
