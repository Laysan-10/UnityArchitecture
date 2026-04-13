public class LoadGameInteractor
{
    private readonly IPlayerSaveRepository _playerRepository;
    private readonly IEnemySaveRepository _enemyRepository;
    private readonly ISaveDataRepository _saveDataRepository;

    public LoadGameInteractor(
        IPlayerSaveRepository playerRepository,
        IEnemySaveRepository enemyRepository,
        ISaveDataRepository saveDataRepository)
    {
        _playerRepository = playerRepository;
        _enemyRepository = enemyRepository;
        _saveDataRepository = saveDataRepository;
    }

    public void Execute()
    {
        SaveData saveData = _saveDataRepository.Load();
        if (saveData == null)
        {
            return;
        }

        _playerRepository.Apply(saveData.Player);
        _enemyRepository.Apply(saveData.EnemyStates);
    }
}
