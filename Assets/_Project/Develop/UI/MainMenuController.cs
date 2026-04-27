using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController
{
    private const string GameplaySceneName = "World-game";

    private readonly MainMenuView _view;
    private readonly SettingsModel _model;
    private readonly IAudioService _audioService;
    private readonly ISaveInteractor _saveInteractor;

    public MainMenuController(
        MainMenuView view,
        SettingsModel model,
        IAudioService audioService,
        ISaveInteractor saveInteractor)
    {
        _view = view;
        _model = model;
        _audioService = audioService;
        _saveInteractor = saveInteractor;

        _view.settingsPanel.SetActive(false);
        _view.volumeSlider.value = _model.MusicVolume;
        _view.SetContinueInteractable(_saveInteractor != null && _saveInteractor.HasSave(GameplaySceneName));

        _view.newGameButton.onClick.AddListener(PlayClick);
        _view.continueButton.onClick.AddListener(PlayClick);
        _view.settingsButton.onClick.AddListener(PlayClick);
        _view.closeSettingsButton.onClick.AddListener(PlayClick);

        if (_view.quitButton != null)
        {
            _view.quitButton.onClick.AddListener(PlayClick);
        }

        _view.newGameButton.onClick.AddListener(StartNewGame);
        _view.continueButton.onClick.AddListener(ContinueGame);
        _view.settingsButton.onClick.AddListener(() => _view.ShowSettings(true));
        _view.closeSettingsButton.onClick.AddListener(() => _view.ShowSettings(false));
        _view.volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void PlayClick()
    {
        _audioService.PlaySound("Button_Click");
    }

    private void StartNewGame()
    {
        _saveInteractor?.DeleteSave(GameplaySceneName);
        _view.SetContinueInteractable(false);
        SceneManager.LoadScene(GameplaySceneName);
    }

    private void ContinueGame()
    {
        if (_saveInteractor == null || !_saveInteractor.HasSave(GameplaySceneName))
        {
            _view.SetContinueInteractable(false);
            return;
        }

        SceneManager.LoadScene(GameplaySceneName);
    }

    private void OnVolumeChanged(float value)
    {
        _model.MusicVolume = value;
        _audioService.SetVolume(value);
    }
}
