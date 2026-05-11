using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectBootstrapper : MonoBehaviour
{
    private static ProjectBootstrapper _instance;

    private IAudioService _audioService;
    private ISaveInteractor _saveInteractor;

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

        IGameSaveRepository gameSaveRepository =
            new GameSaveRepository(Application.persistentDataPath);
        _saveInteractor = new SaveInteractor(gameSaveRepository);

        DependencyContainer container = new DependencyContainer();
        container.Register<IAudioService>(_audioService);
        container.Register<ISaveInteractor>(_saveInteractor);
        AppServices.Initialize(container);

        Debug.Log("Global services initialized.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MonoBehaviour[] behaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour is ISceneBootstrapper sceneBootstrapper)
            {
                sceneBootstrapper.Initialize();
            }
        }
    }
}
