using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;


public abstract class Enemy : MonoBehaviour, IDamageble
{
    [Header("Main Settings")]
    public bool playerChasing;
    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;

    [SerializeField]private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if(health >= 0)
            {
                health = 0;
                Die();
            }
        }
    }
    [Header("Attackx Settings")]
    [SerializeField] private float attackCooldown;
    private bool isCooldown;
    [SerializeField] private int attackDamage;

    private void Awake()
    {
        tag = "Enemy";
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(AttackCooldown());
    }

    private void Update()
    {
        if (isCooldown == false) 
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if(playerChasing == true)
            navMeshAgent.destination = playerTransform.position;
    }

    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
    }

    public virtual void Attack()
    {
        StartCoroutine(AttackCooldown());
    }

    public void Die()
    {
        Debug.Log("���� enemy");
    }

    private IEnumerator AttackCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isCooldown = false;
    }
}

