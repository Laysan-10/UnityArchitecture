using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Rigidbody))] // add this component
[RequireComponent(typeof(AudioSource))]

public class LEnemy : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] AudioClip myClip;
    private NavMeshAgent agent;
    private string myTag = "Player";
    [SerializeField] int Damage = 10;
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = Player.position;    
    }
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == myTag)
        {
            GetComponent<AudioSource>().PlayOneShot(myClip);
            Player.GetComponent <LLevelHeath> ().levelHeath -= Damage; // обратимся к игроку и возьмем компонент LLevelHeath
            Debug.Log("Attack by Enemy");
            
        }
    }
}
