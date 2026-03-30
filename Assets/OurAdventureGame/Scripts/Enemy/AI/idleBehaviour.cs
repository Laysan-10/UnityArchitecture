using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idleBehaviour : StateMachineBehaviour
{
    private float timer;
    
    private Transform player;//Ссылка на объект игрока, а точнее на его Transform
    private float chaseRange = 10;// Радиус, в котором персонаж начнет преследовать игрока.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //Вызывается при входе в состояние. Устанавливает таймер в ноль и находит игрока по тегу "Player".
    {
        timer = 0;
        //Это метод Unity, который ищет объект в сцене по его тегу "Player".
        // в нашем случае рыцарь, на котором тег Player
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Вызывается каждый кадр в состоянии.
        //Увеличивает таймер с учетом времени прошедшего с предыдущего кадра.
        //Если таймер превышает 5 секунд, устанавливает параметр "isPatrolling" аниматора в true.
        //Вычисляет расстояние между текущим положением персонажа и игроком, и если это расстояние меньше chaseRange, устанавливает параметр "isChasing" в true.
        
        //
        //Time.deltaTime это значение времени, прошедшее с последнего кадра
        timer += Time.deltaTime;// Эта строка увеличивает значение переменной timer на время, прошедшее с прошлого кадра.
        if (timer > 5)
            animator.SetBool("isPatrolling",true); // обращение к методу аниматора
            //Это метод аниматора, который устанавливает значение булевой переменной в состоянии аниматора. 
            //Это позволяет управлять переходами между анимационными состояниями
            // анимация патрулирования(ходьбы) включается
        
        //Vector3.Distance это метод Unity, который вычисляет расстояние между двумя точками в трехмерном пространстве.
        //animator.transform.position: Это позиция объекта, управляемого аниматором
        //player.position: Позиция игрока
        float distance = Vector3.Distance(animator.transform.position, player.position);
        if(distance < chaseRange)
            animator.SetBool("isChasing",true);
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //Вызывается при выходе из состояния idleBehaviour. В данном случае, метод пустой и не содержит дополнительной логики.
    {
        
    }
}
