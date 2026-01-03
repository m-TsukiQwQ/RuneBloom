using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string _fileName = "data.game";

    private GameData _gameData;
    private List<ISaveable> _saveableObjects;
    private FileDataHandler _dataHandler;

    public static SaveManager Instance { get; private set; }

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode   )
    {
        _saveableObjects = FindAllSaveableObjects();

        LoadGame();
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _dataHandler.Load();
        if (_gameData == null)
        {
            Debug.Log("No save data found. Initializing new game.");
            NewGame();
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

        _dataHandler.Save(_gameData);
        Debug.Log("Game Saved!");
    }

    private List<ISaveable> FindAllSaveableObjects()
    {
        IEnumerable<ISaveable> saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();  

        return new List<ISaveable>(saveables);
    }
}
