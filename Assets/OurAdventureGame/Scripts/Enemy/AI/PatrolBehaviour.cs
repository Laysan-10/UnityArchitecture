using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : StateMachineBehaviour
{
    private float timer;
    private List<Transform> points = new List<Transform>();// Список точек патрулирования. Это список (массив), который содержит объекты типа Transform
    private NavMeshAgent agent;//Ссылка на компонент NavMeshAgent

    private Transform player; // Ссылка на объект игрока.
    private float chaseRange = 10;//Радиус, в котором персонаж начнет преследовать игрока.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        agent.speed = 1.4f;
        Transform pointsObject = GameObject.FindGameObjectWithTag("Points").transform;//Находит игрока по его тегу и сохраняет ссылку на его Transform.
        foreach (Transform t in pointsObject)
            points.Add(t);
        agent = animator.GetComponent<NavMeshAgent>();// Получает компонент NavMeshAgent у объекта, который содержит аниматор.Это переменная, которая теперь ссылается на компонент NavMeshAgent и используется для управления навигацией.
        agent.SetDestination(points[0].position); //points[0].position: Это позиция первой точки в списке points, к которой будет двигаться NavMeshAgent

        player = GameObject.FindGameObjectWithTag("Player").transform; //Этот метод ищет объект в сцене по его тегу "Player"
        //.transform: Получает компонент Transform объекта, который содержит информацию о его позиции, вращении и масштабе.
        //player: Это переменная, которая теперь ссылается на объект игрока (его Transform).
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance) //remainingDistance - это свойство NavMeshAgent, которое возвращает оставшееся расстояние до цели навигации.
                                                               //stoppingDistance - Это свойство, которое определяет минимальное расстояние, при достижении которого навигационный агент считается приблизившимся к цели и остановится.
            agent.SetDestination(points[Random.Range(0, points.Count)].position);
        //points[Random.Range(0, points.Count)].position: Это случайно выбранная точка из списка points.
        //Random.Range используется для генерации случайного индекса из списка точек патрулирования, а затем извлекается её позиция для передачи в метод SetDestination
        //Если навигационный агент приблизился достаточно близко к текущей цели (remainingDistance меньше stoppingDistance),
        //устанавливается новая случайная точка патрулирования из списка points как следующая цель для агента.

        // 

        timer += Time.deltaTime;
        //Этот блок if проверяет, прошло ли более 10 секунд с начала состояния.
        //Если да, то устанавливает параметр аниматора isPatrolling в false, что может сигнализировать о завершении состояния патрулирования.
        if (timer > 10)
            animator.SetBool("isPatrolling", false);

        float distance = Vector3.Distance(animator.transform.position, player.position);//Этот код определяет расстояние между текущей позицией объекта,
        //управляемого аниматором (animator.transform.position), и позицией игрока рыцаря (player.position).
        if (distance < chaseRange)
            animator.SetBool("isChasing", true);
        //Если расстояние между объектом и игроком меньше заданного радиуса chaseRange,
        //устанавливается параметр аниматора isChasing в true, что, вероятно, переключит состояние на преследование игрока.

        //Этот код определяет расстояние между текущей позицией объекта,
        //управляемого аниматором (animator.transform.position), и позицией игрока (player.position).
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        //Эта строка кода используется для того, чтобы установить цель (destination) навигационного агента (NavMeshAgent) на его текущее положение.
        //Фактически, эта операция является простым способом остановить движение навигационного агента.

        //Когда навигационный агент достигает своей цели (то есть agent.remainingDistance становится меньше или равно agent.stoppingDistance),
        //он автоматически останавливается. Однако иногда, в коде, может потребоваться явно установить его цель в текущее положение для остановки.
        //В данном случае, строка agent.SetDestination(agent.transform.position) выполняет эту задачу, делая агент "перемещение" к своему текущему местоположению,
        //что приводит к его немедленной остановке.
    }
}
