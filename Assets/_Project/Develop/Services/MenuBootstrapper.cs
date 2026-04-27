using UnityEngine;

public class MenuBootstrapper : MonoBehaviour, ISceneBootstrapper
{
    [SerializeField] private MainMenuView _mainMenuView;

    private MainMenuController _menuController;
    private bool _isInitialized;

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _isInitialized = true;

        IAudioService audioService = AppServices.Resolve<IAudioService>();
        ISaveInteractor saveInteractor = AppServices.Resolve<ISaveInteractor>();
        SettingsModel settingsModel = new SettingsModel();

        _menuController = new MainMenuController(
            _mainMenuView,
            settingsModel,
            audioService,
            saveInteractor);

        audioService.PlayMusic("Menu");

        Debug.Log("Scene Entrypoint (Menu): MVC собран и запущен.");
    }
}
