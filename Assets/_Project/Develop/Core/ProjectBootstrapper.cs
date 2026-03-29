using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectBootstrapper : MonoBehaviour
{
    // Статический доступ, чтобы Scene Entrypoint мог найти сервисы
    public static ProjectBootstrapper Instance { get; private set; }

    // Конкретные реализации сервисов (сверху вниз)
    public IAudioService AudioService { get; private set; }
    public ISaveLoadService SaveLoadService { get; private set; }

    private void Awake()
    {
        // Проверка на дубликаты (Singleton)
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Объект живет между сценами

        InitializeServices();
        
        // Переходим в Главное Меню сразу после инициализации
        SceneManager.LoadScene("MainMenu");
    }

    private void InitializeServices()
    {
        // Здесь мы решаем, КАКИЕ конкретно сервисы использовать.
        // Это и есть реализация принципа инверсии зависимостей.
        AudioService = new UnityAudioService();
        SaveLoadService = new FileSaveLoadService();

        Debug.Log("Глобальные сервисы инициализированы.");
    }
}