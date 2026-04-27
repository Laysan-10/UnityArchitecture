using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public EnemyStateMachine StateMachine;
    
    [Header("Settings")]
    public float lookRadius = 15f;
    public float attackRange = 2.5f;
    public float attackCooldown = 1.5f;
    public bool isPeaceful = false; // Мирный режим
    public bool isBoss = false;     // Флаг босса

    [Header("References")]
    public UnityEngine.AI.NavMeshAgent agent;
    public BossAnimationController anim;
    public HealthComponent health;
    public Transform target;

    protected virtual void Start()
    {
        StateMachine = new EnemyStateMachine();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<BossAnimationController>();
        health = GetComponent<HealthComponent>();
        
        // Начальное состояние
        StateMachine.ChangeState(new IdleState(this));
    }

    private void Update() => StateMachine.Update();

    // Метод для доп. балла (ускорение босса)
    public float GetAttackSpeed()
    {
        if (isBoss && health.Core.CurrentHealth < health.Core.MaxHealth * 0.5f) return 0.5f; // Ускоряем кулдаун
        return attackCooldown;
    }
}