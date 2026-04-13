using UnityEngine;

public class MenuBootstrapper : MonoBehaviour, ISceneBootstrapper
{
    [SerializeField] private MainMenuView _mainMenuView;

    private MainMenuController _menuController;
    private bool _isInitialized;

    public void Initialize(ProjectContext projectContext)
    {
        if (_isInitialized)
        {
            return;
        }

        _isInitialized = true;

        IAudioService audioService = projectContext.AudioService;
        SettingsModel settingsModel = new SettingsModel();

        _menuController = new MainMenuController(_mainMenuView, settingsModel, audioService);

        audioService.PlayMusic("Menu");

        Debug.Log("Scene Entrypoint (Menu): MVC собран и запущен.");
    }
}
