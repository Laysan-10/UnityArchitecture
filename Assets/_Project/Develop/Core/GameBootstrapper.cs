using UnityEngine;
using UnityEngine.InputSystem;

public class GameBootstrapper : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private InputActionAsset inputActionAsset;

    [Header("Компоненты Игрока")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerAnimationController playerAnimation;
    [SerializeField] private HealthComponent playerHealth;

    [Header("Компоненты UI")]
    [SerializeField] private MagicCooldownUI magicUI;
    [SerializeField] private HealthBarUI healthBarUI; 
    [SerializeField] private GameOverUI gameOverUI; 


    [Header("Компоненты Камеры")]
    [SerializeField] private ThirdPersonCameraController cameraController;

    private InputService _inputService;

    private void Start()
    {
        _inputService = new InputService(inputActionAsset);
        _inputService.Enable();

        playerMovement.Construct(_inputService);
        playerCombat.Construct(_inputService);
        playerAnimation.Construct(playerMovement, playerCombat, playerHealth);
        
        if (cameraController != null) cameraController.Construct(_inputService);
        
        if (magicUI != null) magicUI.Construct(playerCombat);

        if (healthBarUI != null) healthBarUI.Construct(playerHealth.Core);
        if (magicUI != null) magicUI.Construct(playerCombat);
        
        if (gameOverUI != null) gameOverUI.Construct(playerHealth.Core);

        Debug.Log("Все компоненты инициализированы: Все модули связаны.");
    }

    private void OnDestroy()
    {
        _inputService?.Disable();
    }
}