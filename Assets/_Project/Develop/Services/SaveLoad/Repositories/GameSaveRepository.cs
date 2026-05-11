using System.IO;
using UnityEngine;

public class GameSaveRepository : IGameSaveRepository
{
    private readonly string _basePath;

    public GameSaveRepository(string basePath)
    {
        _basePath = Path.Combine(basePath, "GameSaves");

        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    public void SaveToFile<T>(string fileName, T data)
    {
        string path = GetFilePath(fileName);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log($"Game saved to: {path}");
    }

    public T LoadFromFile<T>(string fileName)
    {
        string path = GetFilePath(fileName);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Save file was not found: {path}");
            return default;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }

    public bool Exists(string fileName)
    {
        return File.Exists(GetFilePath(fileName));
    }

    public void Delete(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public void ClearAll()
    {
        if (!Directory.Exists(_basePath))
        {
            return;
        }

        Directory.Delete(_basePath, true);
        Directory.CreateDirectory(_basePath);
    }

    private string GetFilePath(string fileName)
    {
        return Path.Combine(_basePath, $"{fileName}.json");
    }
}
