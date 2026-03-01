using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : StateMachineBehaviour
{
    private NavMeshAgent _agent; //Ссылка на компонент NavMeshAgent
    private Transform player; //Ссылка на объект игрока.
    private float attackRange = 2; //Радиус для атаки.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent = animator.GetComponent<NavMeshAgent>();//Получает ссылку на компонент NavMeshAgent у объекта, управляемого аниматором.Это переменная, которая теперь ссылается на компонент NavMeshAgent
        _agent.speed = 1.8f;

        player = GameObject.FindGameObjectWithTag("Player").transform;  //Находит игрока по его тегу и сохраняет ссылку на его Transform.
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(player.position);// Устанавливает игрока в качестве цели навигационного агента _agent.
        _agent.stoppingDistance = attackRange;
        float distance = Vector3.Distance(animator.transform.position, player.position);
        
        if(distance < attackRange)//Проверка, находится ли расстояние между аниматором и игроком в радиусе атаки (attackRange).
            animator.SetTrigger("IsAttack");//Устанавливает параметр isAttacking в аниматоре в true.
        
        if (distance > 10)//Проверка, находится ли расстояние между аниматором и игроком больше 10 единиц.
            animator.SetBool("isChasing",false);//Устанавливает параметр isChasing в аниматоре в false.
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(_agent.transform.position);//Устанавливает текущую позицию навигационного агента в качестве его цели.
        //Это действие фактически останавливает движение навигационного агента, так как он пытается переместиться в свою текущую позицию.
        _agent.speed = 2;//_agent получает компонент NavMeshAgent у объекта, управляемого аниматором, и устанавливает скорость движения этого агента на 4
    }
}
