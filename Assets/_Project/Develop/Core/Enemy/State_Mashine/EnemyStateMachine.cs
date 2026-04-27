public class EnemyStateMachine
{
    public IState CurrentState { get; private set; }
    public EnemyStateType CurrentStateType { get; private set; }

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentStateType = newState.StateType;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}
