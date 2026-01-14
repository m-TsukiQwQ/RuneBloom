using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotMenu : MonoBehaviour
{

    [Header("Navigation")]
    [SerializeField] private string _gameSceneName = "Game";


    [Header("Load Game Setup")]
    [SerializeField] private Transform _slotsContainer; // The ScrollView Content
    [SerializeField] private GameObject _saveSlotPrefab;


    public void OnSaveSlotClicked(string profileId)
    {
        SaveManager.Instance.ChangeSelectedProfileId(profileId);

        SceneManager.LoadScene(_gameSceneName);
    }

    public void OnNewGameClicked()
    {
        Debug.Log("Creating");
        // 1. Generate a unique Profile ID based don time so it never conflicts
        string newProfileId = "Profile_" + System.DateTime.Now.Ticks;

        // 2. Tell Manager to swap to this new ID
        SaveManager.Instance.ChangeSelectedProfileId(newProfileId);

        // 3. Force a New Game state (Reset data)
        SaveManager.Instance.NewGame();

        // 4. Start
        SceneManager.LoadScene(_gameSceneName);
    }

    public void OnLoadGameClicked()
    {


        // 1. Clean up old buttons
        foreach (Transform child in _slotsContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. Load all profiles from disk
        Dictionary<string, GameData> profilesGameData = SaveManager.Instance.GetAllProfilesGameData();

        // 3. Create a button for each existing save
        foreach (var profile in profilesGameData)
        {
            GameObject slotObj = Instantiate(_saveSlotPrefab, _slotsContainer);
            SaveSlot slotScript = slotObj.GetComponent<SaveSlot>();

            // Pass the ID (Key) and Data (Value) to the button
            slotScript.Initialize(profile.Key, profile.Value);

            // Add click listener dynamically
            Button btn = slotObj.GetComponent<Button>();
            btn.onClick.AddListener(() => OnSaveSlotButtondClicked(slotScript));
        }
    }

    private void OnSaveSlotButtondClicked(SaveSlot slot)
    {
        Debug.Log("Loading");
        // Load the specific profile attached to this button
        SaveManager.Instance.ChangeSelectedProfileId(slot.GetProfileId());

        SceneManager.LoadScene(_gameSceneName);
    }


}
