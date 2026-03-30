using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject deathMenuRoot; 
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    private HealthCore _playerHealth;
    private InputService _inputService;


    public void Construct(HealthCore core, InputService inputService)
    {
        _playerHealth = core;
        _inputService = inputService;
        _playerHealth.OnDeath += HandleDeath;

        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);

        deathMenuRoot.SetActive(false);
    }

    private void HandleDeath()
    {
        _inputService.Disable();
        Debug.Log("UI: Событие смерти получено! Показываю меню.");
        deathMenuRoot.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f; 
        ProjectBootstrapper.Instance.AudioService.StopMusic();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ExitGame()
    {
        Debug.Log("Выход из игры...");
        Application.Quit();
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
            _playerHealth.OnDeath -= HandleDeath;
    }
}