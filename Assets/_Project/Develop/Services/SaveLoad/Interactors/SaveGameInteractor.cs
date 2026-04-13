public class SaveGameInteractor
{
    private readonly IPlayerSaveRepository _playerRepository;
    private readonly IEnemySaveRepository _enemyRepository;
    private readonly ISaveDataRepository _saveDataRepository;

    public SaveGameInteractor(
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
        SaveData saveData = new SaveData
        {
            Player = _playerRepository.GetState(),
            EnemyStates = _enemyRepository.GetStates()
        };

        _saveDataRepository.Save(saveData);
    }
}
