using UnityEngine;

public interface IInputService
{
    Vector2 MoveInput { get; }      // Данные WASD
    Vector2 LookInput { get; }      // Данные мыши
    bool IsSprinting { get; }       // Зажат ли Shift
}