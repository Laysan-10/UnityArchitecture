using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LWalkEnemyBexavior : StateMachineBehaviour
{
    float timer;
    List<Transform> points = new List<Transform>();
    NavMeshAgent agent;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        Transform pointsObject = GameObject.FindGameObjectWithTag("Points").transform;
        foreach (Transform t in points)
        {
            points.Add(t);
            agent = animator.GetComponent<NavMeshAgent>();
            agent.SetDestination(points[0].position); // объект сначало перемешается к первой точке
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(points[Random.Range(0, points.Count)].position); //объект двигается от точки до точки в рандомном порядке
            timer += Time.deltaTime;
            if(timer > 10)
            {
                animator.SetBool("Walk", true);
            }
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
