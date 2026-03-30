using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonSaveLoadService : ISaveLoadService
{
    private readonly List<ISaveable> _saveables = new List<ISaveable>();
    private readonly string _filePath;

    public JsonSaveLoadService()
    {
        // Путь: C:/Users/Имя/AppData/LocalLow/НазваниеКомпании/НазваниеИгры/save.json
        _filePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    public void RegisterSaveable(ISaveable saveable) => _saveables.Add(saveable);
    
    public void UnregisterSaveable(ISaveable saveable)
    {
        if (_saveables.Contains(saveable))
            _saveables.Remove(saveable);
    }

    public void Save()
    {
        SaveData data = new SaveData();

        // Опрашиваем все зарегистрированные объекты (Игрока, Врагов и т.д.)
        foreach (var saveable in _saveables)
        {
            saveable.PopulateSaveData(data);
        }

        // Превращаем объект в красивую строку JSON
        string json = JsonUtility.ToJson(data, true);
        
        // Записываем в файл
        File.WriteAllText(_filePath, json);
        
        Debug.Log($"Игра сохранена в: {_filePath}");
    }

    public void Load()
    {
        if (!File.Exists(_filePath))
        {
            Debug.LogWarning("Файл сохранения не найден!");
            return;
        }

        // Читаем текст из файла
        string json = File.ReadAllText(_filePath);
        
        // Превращаем текст обратно в объект SaveData
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // Раздаем данные всем подписанным объектам
        foreach (var saveable in _saveables)
        {
            saveable.LoadFromSaveData(data);
        }

        Debug.Log("Игра загружена.");
    }
}