using System;
using System.Collections.Generic;

public class SaveInteractor : ISaveInteractor
{
    private readonly IGameSaveRepository _repository;

    public SaveInteractor(IGameSaveRepository repository)
    {
        _repository = repository;
    }

    public void SaveScene(string sceneName, PlayerSaveData player, List<EnemySaveData> enemies)
    {
        SceneSaveData data = new SceneSaveData
        {
            SceneName = sceneName,
            Player = player ?? new PlayerSaveData(),
            EnemyStates = enemies ?? new List<EnemySaveData>(),
            SaveTimeTicks = DateTime.UtcNow.Ticks
        };

        _repository.SaveToFile(sceneName, data);
    }

    public SceneSaveData LoadScene(string sceneName)
    {
        return _repository.LoadFromFile<SceneSaveData>(sceneName);
    }

    public bool HasSave(string sceneName)
    {
        return _repository.Exists(sceneName);
    }

    public void DeleteSave(string sceneName)
    {
        _repository.Delete(sceneName);
    }

    public void DeleteAllSaves()
    {
        _repository.ClearAll();
    }
}
