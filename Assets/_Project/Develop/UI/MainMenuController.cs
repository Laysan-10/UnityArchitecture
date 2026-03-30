using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuController
{
    private readonly MainMenuView _view;
    private readonly SettingsModel _model;
    private readonly IAudioService _audioService;

public MainMenuController(MainMenuView view, SettingsModel model, IAudioService audio)
{
    _view = view;
    _model = model;
    _audioService = audio;

    _view.settingsPanel.SetActive(false);
    _view.volumeSlider.value = _model.MusicVolume;

    // Подписываем звук на все кнопки
    _view.playButton.onClick.AddListener(() => PlayClick());
    _view.settingsButton.onClick.AddListener(() => PlayClick());
    _view.closeSettingsButton.onClick.AddListener(() => PlayClick());
    if (_view.quitButton != null) _view.quitButton.onClick.AddListener(() => PlayClick());

    // Логика кнопок
    _view.playButton.onClick.AddListener(PlayGame);
    _view.settingsButton.onClick.AddListener(() => _view.ShowSettings(true));
    _view.closeSettingsButton.onClick.AddListener(() => _view.ShowSettings(false));
    _view.volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
}

private void PlayClick()
{
    _audioService.PlaySound("Button_Click");
}

    private void PlayGame()
    {
        SceneManager.LoadScene("World-game"); 
    }

    private void OnVolumeChanged(float value)
    {
        _model.MusicVolume = value;
        _audioService.SetVolume(value);
    }
}