using UnityEngine;

public interface IState
{
    void Enter();    // Срабатывает при переходе в состояние
    void Update();   // Срабатывает каждый кадр
    void Exit();     // Срабатывает перед сменой состояния
}
