using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    public Animator animator;
    public float LookRadius;
    public float attackCooldown = 4f; 
    private float lastAttackTime;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lastAttackTime = -attackCooldown;
        
    }

    private void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance < LookRadius)
        {
            animator.SetBool("IsRun", true);
            if(agent != null)
            {
                agent.SetDestination(target.position);
            }
            if (distance <= agent.stoppingDistance)
            {
                animator.SetBool("IsRun", false);
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    animator.SetTrigger("IsAttack");
                    lastAttackTime = Time.time;
                    LookTarget();
                }
            }
            else
            {
                animator.SetBool("IsRun", true);
            }
        }
        else
        {
            animator.SetBool("IsRun", false);
        }
    }

    void LookTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }
}
