using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : IDisposable
{
    private readonly PauseMenuView _view;
    private readonly PauseMenuModel _model;
    private readonly ISaveInteractor _saveInteractor;
    private readonly PlayerSaveController _playerSaveController;
    private readonly EnemySaveController _enemySaveController;
    private readonly string _sceneName;
    private readonly IInputService _inputService;
    private readonly HealthCore _playerHealth;

    public PauseMenuController(
        PauseMenuView view,
        PauseMenuModel model,
        ISaveInteractor saveInteractor,
        PlayerSaveController playerSaveController,
        EnemySaveController enemySaveController,
        string sceneName,
        IInputService inputService,
        HealthCore playerHealth)
    {
        _view = view;
        _model = model;
        _saveInteractor = saveInteractor;
        _playerSaveController = playerSaveController;
        _enemySaveController = enemySaveController;
        _sceneName = sceneName;
        _inputService = inputService;
        _playerHealth = playerHealth;

        _model.IsPeacefulMode = PeacefulModeService.IsEnabled;
        _view.Show(false);
        _view.SetPeacefulMode(_model.IsPeacefulMode);
        _view.loadButton.interactable = _saveInteractor != null && _saveInteractor.HasSave(_sceneName);

        _view.saveButton.onClick.AddListener(SaveGame);
        _view.loadButton.onClick.AddListener(LoadGame);
        _view.mainMenuButton.onClick.AddListener(GoToMainMenu);
        if (_view.peacefulModeToggle != null)
        {
            _view.peacefulModeToggle.onValueChanged.AddListener(SetPeacefulMode);
        }

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
        _view.SetPeacefulMode(_model.IsPeacefulMode);
        Time.timeScale = _model.IsPaused ? 0f : 1f;

        if (!_playerHealth.IsDead)
        {
            Cursor.lockState = _model.IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = _model.IsPaused;
        }
    }

    private void SetPeacefulMode(bool isEnabled)
    {
        _model.IsPeacefulMode = isEnabled;
        PeacefulModeService.SetEnabled(isEnabled);
    }
    
    private void SaveGame()
    {
        if (_saveInteractor == null)
        {
            return;
        }

        _saveInteractor.SaveScene(
            _sceneName,
            _playerSaveController?.GetPlayerData(),
            _enemySaveController?.GetEnemyData());
        _view.loadButton.interactable = _saveInteractor.HasSave(_sceneName);
        ClosePauseMenu();
    }

    private void LoadGame()
    {
        if (_saveInteractor == null || !_saveInteractor.HasSave(_sceneName))
        {
            return;
        }

        SceneSaveData saveData = _saveInteractor.LoadScene(_sceneName);
        if (saveData == null)
        {
            return;
        }

        _playerSaveController?.ApplyPlayerData(saveData.Player);
        _enemySaveController?.ApplyEnemyData(saveData.EnemyStates);
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

    public void Dispose()
    {
        _view.saveButton.onClick.RemoveListener(SaveGame);
        _view.loadButton.onClick.RemoveListener(LoadGame);
        _view.mainMenuButton.onClick.RemoveListener(GoToMainMenu);
        if (_view.peacefulModeToggle != null)
        {
            _view.peacefulModeToggle.onValueChanged.RemoveListener(SetPeacefulMode);
        }
        _inputService.OnPausePressed -= TogglePause;
        _playerHealth.OnDeath -= ForceClosePause;
    }
}
