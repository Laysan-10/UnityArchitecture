using UnityEngine;

public class FileSaveLoadService : ISaveLoadService
{
    public void Save() => Debug.Log("Игра сохранена в файл.");
    public void Load() => Debug.Log("Данные загружены из файла.");
}
