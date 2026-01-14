using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string _fileName = "data.game";

    [SerializeField] private string _selectedProfileId = "test_profile";

    private string _saveDate;


    private GameData _gameData;
    private List<ISaveable> _saveableObjects;
    private FileDataHandler _dataHandler;

    public static SaveManager Instance { get; private set; }

    public bool HasLoadedData => _gameData != null;
    public GameData CurrentGameData => _gameData;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            SaveGame();
        }
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName);
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode   )
    {
        _saveableObjects = FindAllSaveableObjects();

        LoadGame();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        // Call this from Main Menu buttons: "Slot 1", "Slot 2", etc.
        _selectedProfileId = newProfileId;

        // Reload game data for this new profile
        LoadGame();
    }


    public void NewGame(int worldSeed, string worldName = "New World")
    {
        Debug.Log(worldName);
        _gameData = new GameData();
        _gameData.worldName = worldName;
        _gameData.worldSeed = worldSeed;
    }

    public void LoadGame()
    {
        _gameData = _dataHandler.Load(_selectedProfileId);
        if (_gameData == null)
        {
            Debug.Log("No save data found. Initializing new game.");
            NewGame(Math.Abs((int)System.DateTime.Now.Ticks));
        }

        foreach (ISaveable saveable in _saveableObjects)
        {
            saveable.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        foreach(ISaveable saveable in _saveableObjects)
        {
            saveable.SaveData(ref _gameData);
        }

        DateTime localDate = DateTime.Now;
        string cultureName = "ru-RU";
        var culture = new CultureInfo(cultureName);

        string saveTime = localDate.ToString(culture);
        _gameData.saveDate = saveTime;


        _dataHandler.Save(_gameData, _selectedProfileId);
        Debug.Log("Game Saved!");
    }

    private List<ISaveable> FindAllSaveableObjects()
    {
        IEnumerable<ISaveable> saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();  

        return new List<ISaveable>(saveables);
    }

    public void RegisterSaveable(ISaveable saveable)
    {
        if (!_saveableObjects.Contains(saveable))
        {
            _saveableObjects.Add(saveable);
        }
    }

    public void UnregisterSaveable(ISaveable saveable)
    {
        if (_saveableObjects.Contains(saveable))
        {
            _saveableObjects.Remove(saveable);
        }
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return _dataHandler.LoadAllProfiles();
    }
}
