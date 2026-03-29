using UnityEngine;

public class MenuBootstrapper : MonoBehaviour
{
    [SerializeField] private MainMenuView _mainMenuView;

    private MainMenuController _menuController;

    private void Start()
    {
        // 1. Получаем глобальные зависимости (DIP)
        IAudioService audioService = ProjectBootstrapper.Instance.AudioService;

        // 2. Создаем Модель (Данные)
        SettingsModel settingsModel = new SettingsModel();

        // 3. Создаем Контроллер (Логика)
        // Он не MonoBehaviour, поэтому мы создаем его через new
        _menuController = new MainMenuController(_mainMenuView, settingsModel, audioService);
    
        audioService.PlayMusic("Menu"); 

        Debug.Log("Scene Entrypoint (Menu): MVC собран и запущен.");
        
        
        
    }
}