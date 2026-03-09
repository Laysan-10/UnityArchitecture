using System;
using UnityEngine;

public interface IInputService
{
    Vector2 MoveInput { get; }
    bool IsSprinting { get; }

     float ZoomInput { get; } 
    
    // События для разовых действий (атаки)
    event Action OnPhysicalAttack;
    event Action OnMagicAttack;
}