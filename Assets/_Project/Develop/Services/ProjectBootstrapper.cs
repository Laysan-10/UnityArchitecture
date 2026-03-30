using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectBootstrapper : MonoBehaviour
{
    public static ProjectBootstrapper Instance { get; private set; }

    public IAudioService AudioService { get; private set; }
    public ISaveLoadService SaveLoadService { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeServices();
        
        SceneManager.LoadScene("MainMenu");
    }

    private void InitializeServices()
    {
        AudioService = new UnityAudioService();
        SaveLoadService = new JsonSaveLoadService(); 

        Debug.Log("Глобальные сервисы инициализированы.");
    }
}