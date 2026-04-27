using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LEnemyPatrol : MonoBehaviour
{
    NavMeshAgent agent;
    int i;
    [SerializeField] List<Transform> targets;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

   void TargetUpdate()
    {
        i = Random.Range(0, targets.Count);
    }
    void Update()
    {
        if(agent.transform.position == agent.pathEndPosition)
        {
            TargetUpdate();
        }
        agent.SetDestination(targets[i].position);
        
    }
}
