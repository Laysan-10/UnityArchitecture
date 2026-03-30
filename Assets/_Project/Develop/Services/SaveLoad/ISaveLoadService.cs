public interface ISaveLoadService
{
    void Save();
    void Load();
    
   void RegisterSaveable(ISaveable saveable);
   void UnregisterSaveable(ISaveable saveable);
}