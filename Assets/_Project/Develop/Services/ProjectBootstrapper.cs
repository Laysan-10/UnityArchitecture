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
        _saveLoadService = new JsonSaveLoadService();
        _projectContext = new ProjectContext(_audioService, _saveLoadService);

        Debug.Log("Глобальные сервисы инициализированы.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MonoBehaviour[] behaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var behaviour in behaviours)
        {
            if (behaviour is ISceneBootstrapper sceneBootstrapper)
            {
                sceneBootstrapper.Initialize(_projectContext);
            }
        }
    }
}
