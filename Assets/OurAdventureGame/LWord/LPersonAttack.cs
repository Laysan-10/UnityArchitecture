using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPersonAttack : MonoBehaviour
{
    
    [SerializeField] Transform Enemy;

    private int PersonDamage = 4;
    private bool check;
    private float BeginTime;
    private float EndTime;
    void Start()
    {
        check = false;
        
    }

    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetKey(KeyCode.E))
        {
            check = true;
            BeginTime = Time.time;
        }
        //Debug.Log(BeginTime);
        //Debug.Log(check);
        EndTime = Time.time;
        if(check && ((EndTime - BeginTime) >= 1.5)) {
            check = false;    
        }
        
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Enemy" && check)
        {


            Enemy.GetComponent<L2MovementEnemy>().HeathEnemy -= PersonDamage; // обратимся к игроку и возьмем компонент LLevelHeath
            Debug.Log("PersonAttack");

        }

    }
}
