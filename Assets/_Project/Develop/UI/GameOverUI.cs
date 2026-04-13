using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject deathMenuRoot;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    private HealthCore _playerHealth;
    private InputService _inputService;
    private IAudioService _audioService;

    public void Construct(HealthCore core, InputService inputService, IAudioService audioService)
    {
        _playerHealth = core;
        _inputService = inputService;
        _audioService = audioService;
        _playerHealth.OnDeath += HandleDeath;

        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);

        deathMenuRoot.SetActive(false);
    }

    private void HandleDeath()
    {
        _inputService.Disable();
        Debug.Log("UI: player death received, showing game over menu.");
        deathMenuRoot.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        _audioService?.StopMusic();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
            _playerHealth.OnDeath -= HandleDeath;
    }
}
