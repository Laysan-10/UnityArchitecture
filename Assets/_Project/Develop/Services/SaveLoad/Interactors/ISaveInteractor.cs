using System.Collections.Generic;

public interface ISaveInteractor
{
    void SaveScene(string sceneName, PlayerSaveData player, List<EnemySaveData> enemies);
    SceneSaveData LoadScene(string sceneName);
    bool HasSave(string sceneName);
    void DeleteSave(string sceneName);
    void DeleteAllSaves();
}
