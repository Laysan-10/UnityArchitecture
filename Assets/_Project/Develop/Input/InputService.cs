using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : IInputService
{
    public Vector2 MoveInput { get; private set; }
    public bool IsSSprint { get; private set; }
    public bool IsSprinting { get; private set; }
    
    public float ZoomInput { get; private set; }

    public event Action OnPhysicalAttack;
    public event Action OnMagicAttack;

    private readonly InputActionMap _playerActionMap;
    private readonly InputAction _moveAction;
    private readonly InputAction _sprintAction;
    private readonly InputAction _physAttackAction;
    private readonly InputAction _magicAttackAction;
    
    private readonly InputAction _mouseZoomAction;

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
        
        _mouseZoomAction = _playerActionMap.FindAction("MouseZoom");

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

        if (_physAttackAction != null) 
            _physAttackAction.performed += ctx => OnPhysicalAttack?.Invoke();

        if (_magicAttackAction != null) 
            _magicAttackAction.performed += ctx => OnMagicAttack?.Invoke();

        if (_mouseZoomAction != null)
        {
            _mouseZoomAction.performed += ctx => 
                ZoomInput = Mathf.Clamp(ctx.ReadValue<Vector2>().y, -1f, 1f);

            _mouseZoomAction.canceled += ctx => 
                ZoomInput = 0f;
        }
    }

    public void Enable() => _playerActionMap?.Enable();
    public void Disable() => _playerActionMap?.Disable();
}