using UnityEngine;
using UnityEngine.InputSystem;

public class GameBootstrapper : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InputActionAsset inputActionAsset;

    [Header("Player Components")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerAnimationController playerAnimation;
    [SerializeField] private HealthComponent playerHealth;

    [Header("UI Components")]
    [SerializeField] private MagicCooldownUI magicUI;
    [SerializeField] private HealthBarUI healthBarUI; 
    [SerializeField] private GameOverUI gameOverUI; 
    
    [Header("UI Pause Menu")]
    [SerializeField] private PauseMenuView pauseMenuView;



    [Header("Camera Components")]
    [SerializeField] private ThirdPersonCameraController cameraController;
    private PauseMenuController _pauseMenuController;


    private InputService _inputService;

    private void Start()
    {
        // Запрашиваем глобальные сервисы у Entrypoint проекта
        var audio = ProjectBootstrapper.Instance.AudioService;
        var saveService = ProjectBootstrapper.Instance.SaveLoadService;

        saveService.RegisterSaveable(playerMovement);
        saveService.RegisterSaveable(playerHealth);

        _inputService = new InputService(inputActionAsset);
        _inputService.Enable();

        playerMovement.Construct(_inputService);
        playerCombat.Construct(_inputService, playerHealth);
        playerAnimation.Construct(playerMovement, playerCombat, playerHealth);
        
        if (cameraController != null) cameraController.Construct(_inputService);
        if (magicUI != null) magicUI.Construct(playerCombat);
        if (healthBarUI != null) healthBarUI.Construct(playerHealth.Core);
        if (gameOverUI != null) gameOverUI.Construct(playerHealth.Core, _inputService);

        audio.PlayMusic("MainTheme");
        audio.PlaySound("Game_Start");

        newEnemyAI[] allEnemies = FindObjectsByType<newEnemyAI>(FindObjectsSortMode.None);
        foreach (var enemy in allEnemies)
        {
            saveService.RegisterSaveable(enemy);
        }

        _pauseMenuController = new PauseMenuController(pauseMenuView, new PauseMenuModel(), saveService, _inputService, playerHealth.Core);

    }

    private void OnDestroy()
    {
        _inputService?.Disable();
    }
}