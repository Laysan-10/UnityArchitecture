using UnityEngine.SceneManagement;

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

        // Подписываемся на кнопки
        _view.playButton.onClick.AddListener(PlayGame);
        _view.settingsButton.onClick.AddListener(() => _view.ShowSettings(true));
        _view.closeSettingsButton.onClick.AddListener(() => _view.ShowSettings(false));
        
        // Настройка слайдера
        _view.volumeSlider.value = _model.MusicVolume;
        _view.volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); // Имя вашей игровой сцены
    }

    private void OnVolumeChanged(float value)
    {
        _model.MusicVolume = value;
        _audioService.SetVolume(value);
    }
}