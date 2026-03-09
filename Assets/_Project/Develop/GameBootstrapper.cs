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

    private InputService _inputService;

    private void Awake()
    {
        // 1. СОЗДАЕМ СЕРВИСЫ (Настраиваем Инфраструктуру)
        _inputService = new InputService(inputActionAsset);
        _inputService.Enable();

        // 2. ВНЕДРЯЕМ ЗАВИСИМОСТИ (Dependency Injection)
        // Передаем ввод в логику
        playerMovement.Construct(_inputService);
        playerCombat.Construct(_inputService);

        // Передаем логику в аниматор (Аниматор просто наблюдает за логикой)
        playerAnimation.Construct(playerMovement, playerCombat);

        Debug.Log("Архитектура инициализирована: Ввод и Аниматор связаны.");
    }

    private void OnDestroy()
    {
        _inputService?.Disable();
    }
}