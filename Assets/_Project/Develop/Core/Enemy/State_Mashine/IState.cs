public interface IState
{
    EnemyStateType StateType { get; }
    void Enter();
    void Update();
    void Exit();
}
