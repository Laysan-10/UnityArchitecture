using System.IO;
using UnityEngine;

public class JsonSaveDataRepository : ISaveDataRepository
{
    private readonly string _filePath;

    public JsonSaveDataRepository()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    public void Save(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(_filePath, json);
        Debug.Log($"Game saved to: {_filePath}");
    }

    public SaveData Load()
    {
        if (!File.Exists(_filePath))
        {
            Debug.LogWarning("Save file was not found.");
            return null;
        }

        string json = File.ReadAllText(_filePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        return saveData ?? new SaveData();
    }
}
