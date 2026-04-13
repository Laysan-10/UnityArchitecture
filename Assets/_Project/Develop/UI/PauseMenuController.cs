using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController
{
    private readonly PauseMenuView _view;
    private readonly PauseMenuModel _model;
    private readonly ISaveLoadService _saveService;
    private readonly IInputService _inputService;
    private readonly HealthCore _playerHealth;

    public PauseMenuController(PauseMenuView view, PauseMenuModel model, ISaveLoadService saveService, IInputService inputService, HealthCore playerHealth)
    {
        _view = view;
        _model = model;
        _saveService = saveService;
        _inputService = inputService;
        _playerHealth = playerHealth;

        _view.Show(false);

        _view.saveButton.onClick.AddListener(SaveGame);
        _view.loadButton.onClick.AddListener(LoadGame);
        _view.mainMenuButton.onClick.AddListener(GoToMainMenu);

        _inputService.OnPausePressed += TogglePause;
        
        _playerHealth.OnDeath += ForceClosePause;
    }

    private void TogglePause()
    {
        if (_playerHealth.IsDead) return;

        _model.IsPaused = !_model.IsPaused;
        UpdatePauseState();
    }

    private void ForceClosePause()
    {
        _model.IsPaused = false;
        UpdatePauseState();
    }

    private void UpdatePauseState()
    {
        _view.Show(_model.IsPaused);
        Time.timeScale = _model.IsPaused ? 0f : 1f;

        if (!_playerHealth.IsDead)
        {
            Cursor.lockState = _model.IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = _model.IsPaused;
        }
    }
    
    private void SaveGame()
    {
        _saveService.Save();
        ClosePauseMenu();
    }

    private void LoadGame()
    {
        _saveService.Load();
        ClosePauseMenu();
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void ClosePauseMenu()
    {
        _model.IsPaused = false;
        UpdatePauseState();
    }
}
