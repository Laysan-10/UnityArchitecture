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


    [Header("Camera Components")]
    [SerializeField] private ThirdPersonCameraController cameraController;

    private InputService _inputService;

    private void Start()
    {
        // Запрашиваем глобальные сервисы у Entrypoint проекта
        var audio = ProjectBootstrapper.Instance.AudioService;
        var save = ProjectBootstrapper.Instance.SaveLoadService;

        // Инициализируем локальный ввод
        _inputService = new InputService(inputActionAsset);
        _inputService.Enable();

        // Дальше твоя обычная логика прокидывания зависимостей (Construct)
        playerMovement.Construct(_inputService);
        playerCombat.Construct(_inputService);
        playerAnimation.Construct(playerMovement, playerCombat, playerHealth);
        
        if (cameraController != null) cameraController.Construct(_inputService);
        if (magicUI != null) magicUI.Construct(playerCombat);
        if (healthBarUI != null) healthBarUI.Construct(playerHealth.Core);
        if (gameOverUI != null) gameOverUI.Construct(playerHealth.Core);

        // Теперь мы можем проиграть стартовый звук через сервис!
        audio.PlaySound("Game_Start");
    }

    private void OnDestroy()
    {
        _inputService?.Disable();
    }
}