using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameBootstrapper : MonoBehaviour, ISceneBootstrapper
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
    private readonly List<ISaveable> _registeredSaveables = new();
    private ISaveLoadService _saveLoadService;
    private bool _isInitialized;

    public void Initialize(ProjectContext projectContext)
    {
        if (_isInitialized)
        {
            return;
        }

        _isInitialized = true;

        IAudioService audioService = projectContext.AudioService;
        _saveLoadService = projectContext.SaveLoadService;

        _inputService = new InputService(inputActionAsset);
        _inputService.Enable();

        playerMovement.Construct(_inputService);
        playerHealth.Construct(audioService);
        playerCombat.Construct(_inputService, playerHealth, audioService);
        playerAnimation.Construct(playerMovement, playerCombat, playerHealth);

        if (cameraController != null) cameraController.Construct(_inputService);
        if (magicUI != null) magicUI.Construct(playerCombat);
        if (healthBarUI != null) healthBarUI.Construct(playerHealth.Core);
        if (gameOverUI != null) gameOverUI.Construct(playerHealth.Core, _inputService, audioService);

        audioService.PlayMusic("MainTheme");
        audioService.PlaySound("Game_Start");

        RegisterSaveable(playerMovement);
        RegisterSaveable(playerHealth);

        newEnemyAI[] allEnemies = FindObjectsByType<newEnemyAI>(FindObjectsSortMode.None);
        foreach (var enemy in allEnemies)
        {
            enemy.Construct(audioService);
            RegisterSaveable(enemy);
        }

        _pauseMenuController = new PauseMenuController(
            pauseMenuView,
            new PauseMenuModel(),
            _saveLoadService,
            _inputService,
            playerHealth.Core);
    }

    private void RegisterSaveable(ISaveable saveable)
    {
        _saveLoadService.RegisterSaveable(saveable);
        _registeredSaveables.Add(saveable);
    }

    private void OnDestroy()
    {
        if (_saveLoadService != null)
        {
            foreach (var saveable in _registeredSaveables)
            {
                _saveLoadService.UnregisterSaveable(saveable);
            }
        }

        _inputService?.Disable();
    }
}
