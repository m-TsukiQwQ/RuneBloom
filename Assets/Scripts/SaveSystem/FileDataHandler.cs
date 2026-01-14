using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string _dataDirPath = "";
    private string _dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        _dataDirPath = dataDirPath;
        _dataFileName = dataFileName;
    }

    public GameData Load(string profileId)
    {
        string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = File.ReadAllText(fullPath);

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data, string profileId)
    {
        string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            File.WriteAllText(fullPath, dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        // Loop over all directory names in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(_dataDirPath).EnumerateDirectories();

        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            // Defensive: Check if data file exists inside this folder
            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning($"Skipping profile '{profileId}' - data file not found.");
                continue;
            }

            // Load the data just to read metadata (like timestamp or player level)
            GameData profileData = Load(profileId);
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
        }

        return profileDictionary;
    }
}
