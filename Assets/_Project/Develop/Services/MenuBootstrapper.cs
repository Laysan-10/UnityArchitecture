using UnityEngine;

public class MenuBootstrapper : MonoBehaviour
{
    [SerializeField] private MainMenuView _mainMenuView;

    private MainMenuController _menuController;

    private void Start()
    {
        IAudioService audioService = ProjectBootstrapper.Instance.AudioService;

        SettingsModel settingsModel = new SettingsModel();

        _menuController = new MainMenuController(_mainMenuView, settingsModel, audioService);
    
        audioService.PlayMusic("Menu"); 

        Debug.Log("Scene Entrypoint (Menu): MVC собран и запущен.");
    }
}