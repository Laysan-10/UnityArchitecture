using UnityEngine;

public class AttackState : BaseAttackState
{
    public AttackState(EnemyBase enemy) : base(enemy)
    {
    }

    public override EnemyStateType StateType => EnemyStateType.Attack;
    protected override EnemyStateType AggressiveStateType => EnemyStateType.Aggressive;

    protected override void PerformAttack()
    {
        Enemy.PerformAttack();
    }
}
