using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectBootstrapper : MonoBehaviour
{
    private static ProjectBootstrapper _instance;

    private IAudioService _audioService;
    private ISaveLoadService _saveLoadService;

    private ProjectContext _projectContext;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeServices();
        SceneManager.sceneLoaded += OnSceneLoaded;

        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _instance = null;
        }
    }

    private void InitializeServices()
    {
        _audioService = new UnityAudioService();

        IPlayerSaveRepository playerRepository = new PlayerSaveRepository();
        IEnemySaveRepository enemyRepository = new EnemySaveRepository();
        ISaveDataRepository saveDataRepository = new JsonSaveDataRepository();
        SaveGameInteractor saveInteractor =
            new SaveGameInteractor(playerRepository, enemyRepository, saveDataRepository);
        LoadGameInteractor loadInteractor =
            new LoadGameInteractor(playerRepository, enemyRepository, saveDataRepository);

        _saveLoadService = new SaveLoadService(
            playerRepository,
            enemyRepository,
            saveInteractor,
            loadInteractor);

        _projectContext = new ProjectContext(_audioService, _saveLoadService);

        Debug.Log("Global services initialized.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MonoBehaviour[] behaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour is ISceneBootstrapper sceneBootstrapper)
            {
                sceneBootstrapper.Initialize(_projectContext);
            }
        }
    }
}
