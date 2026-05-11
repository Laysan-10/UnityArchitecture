using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    private PlayerController _playerController;
    private InputService _inputService;
    private PlayerSaveController _playerSaveController;
    private EnemySaveController _enemySaveController;
    private ISaveInteractor _saveInteractor;
    private bool _isInitialized;

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _isInitialized = true;

        IAudioService audioService = AppServices.Resolve<IAudioService>();
        _saveInteractor = AppServices.Resolve<ISaveInteractor>();

        _inputService = new InputService(inputActionAsset);
        _inputService.Enable();

        playerMovement.Construct(_inputService);
        playerHealth.Construct(audioService);
        playerCombat.Construct(_inputService, playerHealth, audioService);
        _playerController = new PlayerController(
            new PlayerModel(),
            playerAnimation,
            playerMovement,
            playerCombat,
            playerHealth);

        if (cameraController != null) cameraController.Construct(_inputService);
        if (magicUI != null) magicUI.Construct(playerCombat);
        if (healthBarUI != null) healthBarUI.Construct(playerHealth.Core);
        if (gameOverUI != null) gameOverUI.Construct(playerHealth.Core, _inputService, audioService);

        audioService.PlayMusic("MainTheme");
        audioService.PlaySound("Game_Start");

        EnemyBase[] allEnemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        foreach (EnemyBase enemy in allEnemies)
        {
            enemy.Construct(audioService);
        }

        _playerSaveController = new PlayerSaveController(playerMovement, playerHealth);
        _enemySaveController = new EnemySaveController(allEnemies);

        string sceneName = SceneManager.GetActiveScene().name;
        InitializeSceneState(sceneName);

        _pauseMenuController = new PauseMenuController(
            pauseMenuView,
            new PauseMenuModel(),
            _saveInteractor,
            _playerSaveController,
            _enemySaveController,
            sceneName,
            _inputService,
            playerHealth.Core);
    }

    private void Update()
    {
        _playerController?.Tick();
    }

    private void InitializeSceneState(string sceneName)
    {
        if (_saveInteractor == null)
        {
            _playerSaveController?.ApplyDefaultState();
            _enemySaveController?.ApplyDefaultState();
            return;
        }

        if (_saveInteractor.HasSave(sceneName))
        {
            SceneSaveData saveData = _saveInteractor.LoadScene(sceneName);
            if (saveData != null)
            {
                _playerSaveController?.ApplyPlayerData(saveData.Player);
                _enemySaveController?.ApplyEnemyData(saveData.EnemyStates);
                return;
            }
        }

        _playerSaveController?.ApplyDefaultState();
        _enemySaveController?.ApplyDefaultState();
    }

    private void OnDestroy()
    {
        _pauseMenuController?.Dispose();
        _playerController?.Dispose();
        _inputService?.Disable();
    }
}
