using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : IInputService
{
    public Vector2 MoveInput { get; private set; }
    public bool IsSSprint { get; private set; }
    public bool IsSprinting { get; private set; }
    
    // Реализация свойства ZoomInput
    public float ZoomInput { get; private set; }

    public event Action OnPhysicalAttack;
    public event Action OnMagicAttack;

    private readonly InputActionMap _playerActionMap;
    private readonly InputAction _moveAction;
    private readonly InputAction _sprintAction;
    private readonly InputAction _physAttackAction;
    private readonly InputAction _magicAttackAction;
    
    // Экшены для камеры
    private readonly InputAction _mouseZoomAction;
    private readonly InputAction _gamepadZoomAction;

    public InputService(InputActionAsset inputAsset)
    {
        if (inputAsset == null)
        {
            Debug.LogError("InputAsset is NULL!");
            return;
        }

        _playerActionMap = inputAsset.FindActionMap("Player");
        
        _moveAction = _playerActionMap.FindAction("Move");
        _sprintAction = _playerActionMap.FindAction("Sprint");
        _physAttackAction = _playerActionMap.FindAction("Physical_attack");
        _magicAttackAction = _playerActionMap.FindAction("Magic_attack");
        
        // Ищем новые экшены камеры
        _mouseZoomAction = _playerActionMap.FindAction("MouseZoom");
        _gamepadZoomAction = _playerActionMap.FindAction("GamepadZoom");

        // Подписки на перемещение и бег
        if (_moveAction != null)
        {
            _moveAction.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            _moveAction.canceled += ctx => MoveInput = Vector2.zero;
        }

        if (_sprintAction != null)
        {
            _sprintAction.performed += ctx => IsSprinting = true;
            _sprintAction.canceled += ctx => IsSprinting = false;
        }

        if (_physAttackAction != null) _physAttackAction.performed += ctx => OnPhysicalAttack?.Invoke();
        if (_magicAttackAction != null) _magicAttackAction.performed += ctx => OnMagicAttack?.Invoke();

        // Подписки на зум (Считываем каждый кадр через Update зума в камере, поэтому используем лямбды для чтения значения)
        if (_mouseZoomAction != null)
        {
            _mouseZoomAction.performed += ctx => CalculateZoom(ctx.ReadValue<Vector2>().y, 0);
            _mouseZoomAction.canceled += ctx => CalculateZoom(0, _gamepadZoomAction?.ReadValue<float>() ?? 0);
        }
        
        if (_gamepadZoomAction != null)
        {
            _gamepadZoomAction.performed += ctx => CalculateZoom(0, ctx.ReadValue<float>());
            _gamepadZoomAction.canceled += ctx => CalculateZoom(_mouseZoomAction?.ReadValue<Vector2>().y ?? 0, 0);
        }
    }

    // Логика объединения мыши (значения типа 120, -120) и геймпада (значения -1, 1)
    private void CalculateZoom(float mouseScrollY, float gamepadZoom)
    {
        if (mouseScrollY != 0) ZoomInput = Mathf.Clamp(mouseScrollY, -1f, 1f); // Нормализуем колесико мыши
        else if (gamepadZoom != 0) ZoomInput = gamepadZoom;
        else ZoomInput = 0f;
    }

    public void Enable() => _playerActionMap?.Enable();
    public void Disable() => _playerActionMap?.Disable();
}