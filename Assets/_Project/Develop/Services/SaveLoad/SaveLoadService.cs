public class SaveLoadService : ISaveLoadService
{
    private readonly IPlayerSaveRepository _playerRepository;
    private readonly IEnemySaveRepository _enemyRepository;
    private readonly SaveGameInteractor _saveInteractor;
    private readonly LoadGameInteractor _loadInteractor;

    public SaveLoadService(
        IPlayerSaveRepository playerRepository,
        IEnemySaveRepository enemyRepository,
        SaveGameInteractor saveInteractor,
        LoadGameInteractor loadInteractor)
    {
        _playerRepository = playerRepository;
        _enemyRepository = enemyRepository;
        _saveInteractor = saveInteractor;
        _loadInteractor = loadInteractor;
    }

    public void Save() => _saveInteractor.Execute();

    public void Load() => _loadInteractor.Execute();

    public void BindPlayer(PlayerMovement playerMovement, HealthComponent playerHealth)
    {
        _playerRepository.Bind(playerMovement, playerHealth);
    }

    public void ClearPlayer()
    {
        _playerRepository.Clear();
    }

    public void RegisterEnemy(newEnemyAI enemy)
    {
        _enemyRepository.Register(enemy);
    }

    public void UnregisterEnemy(newEnemyAI enemy)
    {
        _enemyRepository.Unregister(enemy);
    }

    public void ClearEnemies()
    {
        _enemyRepository.Clear();
    }
}
