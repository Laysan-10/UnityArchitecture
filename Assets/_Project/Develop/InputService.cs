using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : IInputService
{
    private GameControls _controls; // Сгенерированный класс

    public Vector2 MoveInput => _controls.Player.Move.ReadValue<Vector2>();
    public Vector2 LookInput => _controls.Player.Look.ReadValue<Vector2>();
    public bool IsSprinting => _controls.Player.Sprint.IsPressed();

    public InputService()
    {
        _controls = new GameControls();
        _controls.Enable();
    }
    
    // Не забываем отключать инпут при выходе/смерти
    public void Cleanup()
    {
        _controls.Disable();
    }
}