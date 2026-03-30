using System;
using UnityEngine;

public interface IInputService
{
    Vector2 MoveInput { get; }
    bool IsSprinting { get; }

     float ZoomInput { get; } 
    
    event Action OnPhysicalAttack;
    event Action OnMagicAttack;
    event Action OnPausePressed;
}