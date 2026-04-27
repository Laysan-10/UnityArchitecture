using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    private Transform player;//Ссылка на объект игрока.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;//Находит игрока по его тегу и сохраняет ссылку на его Transform.
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(player); //Метод, который поворачивает объект в сторону цели, переданной как параметр (в данном случае, объект player)
                                           //Поворачивает объект, управляемый аниматором, так чтобы он смотрел на игрока
        
        float distance = Vector3.Distance(animator.transform.position, player.position);//Вычисляет расстояние между объектом, управляемым аниматором, и игроком.
        //if(distance > 3) //Если расстояние до игрока больше 3 единиц
        //    animator.SetBool("isAttacking",false); // устанавливает параметр аниматора isAttacking в false
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
