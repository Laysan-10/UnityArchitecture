// Интерфейс для объектов, которые имеют данные для сохранения
public interface ISaveable
{
    void PopulateSaveData(SaveData saveData); 
    void LoadFromSaveData(SaveData saveData);
}