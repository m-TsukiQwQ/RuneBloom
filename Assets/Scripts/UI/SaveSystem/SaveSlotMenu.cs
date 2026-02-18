using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WorldTime;

public class SaveSlotMenu : MonoBehaviour
{

    [Header("Navigation")]
    [SerializeField] private string _gameSceneName = "Game";
    [Space]

    [Header("New Game Setup")]
    [SerializeField] private TMP_InputField _saveNameInput;
    [SerializeField] private TMP_InputField _seedInput;
    [Space]

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
        string worldName = "New World";
        if (_saveNameInput != null && !string.IsNullOrWhiteSpace(_saveNameInput.text))
        {
            worldName = _saveNameInput.text;
        }

        int worldSeed = Math.Abs((int)System.DateTime.Now.Ticks);
        if (_seedInput != null && !string.IsNullOrWhiteSpace(_seedInput.text))
        {
            worldSeed = int.Parse(_seedInput.text);
        }



        Debug.Log("Creating");
        // 1. Generate a unique Profile ID based don time so it never conflicts
        string newProfileId = "Profile_" + System.DateTime.Now.Ticks;

        // 2. Tell Manager to swap to this new ID
        SaveManager.Instance.ChangeSelectedProfileId(newProfileId);

        // 3. Force a New Game state (Reset data)

        Debug.Log(worldName);
        SaveManager.Instance.NewGame(worldSeed,worldName);

        SaveManager.Instance.SaveGame();

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

            // Pass the ID and Data  to the button
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
