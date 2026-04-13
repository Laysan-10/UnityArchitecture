public interface ISaveDataRepository
{
    void Save(SaveData saveData);
    SaveData Load();
}
