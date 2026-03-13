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

    public void Construct(HealthCore core)
    {
        _playerHealth = core;
        _playerHealth.OnDeath += HandleDeath;

        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);

        deathMenuRoot.SetActive(false);
    }

    private void HandleDeath()
    {
        Debug.Log("UI: Событие смерти получено! Показываю меню.");
        deathMenuRoot.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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