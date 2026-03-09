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
    
    [Header("Компоненты Камеры")]
    [SerializeField] private ThirdPersonCameraController cameraController; // <-- ДОБАВИЛИ

    private InputService _inputService;

    private void Awake()
    {
        _inputService = new InputService(inputActionAsset);
        _inputService.Enable();

        playerMovement.Construct(_inputService);
        playerCombat.Construct(_inputService);
        playerAnimation.Construct(playerMovement, playerCombat);
        
        // Внедряем зависимость ввода в камеру
        if (cameraController != null) cameraController.Construct(_inputService);

        Debug.Log("Архитектура инициализирована: Ввод, Аниматор и Камера связаны.");
    }

    private void OnDestroy()
    {
        _inputService?.Disable();
    }
}