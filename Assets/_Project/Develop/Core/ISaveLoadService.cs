public interface ISaveLoadService
{
    void Save();
    void Load();
    
    // Метод, чтобы игрок мог сказать сервису: "Эй, сохрани и мои данные тоже"
    void RegisterSaveable(ISaveable saveable);
}