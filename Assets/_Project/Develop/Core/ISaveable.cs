// Интерфейс для объектов, которые имеют данные для сохранения
public interface ISaveable
{
    void PopulateSaveData(SaveData saveData); // Записать свои данные в общий файл
    void LoadFromSaveData(SaveData saveData); // Считать свои данные из файла
}