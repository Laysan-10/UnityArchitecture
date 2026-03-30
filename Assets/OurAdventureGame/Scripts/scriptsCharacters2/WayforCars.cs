using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayforCars : MonoBehaviour
{
    public List<GameObject> Waypoints;
    public float speed = 2;
    int index = 0;
    public bool IsLoop = true;
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 destination = Waypoints[index].transform.position;
        Vector3 NewPos = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        transform.position = NewPos;
        float distance = Vector3.Distance(transform.position, destination);
        Vector3 rotate = transform.eulerAngles;



        if (distance <= 0.05)
        {
            if (index < Waypoints.Count - 1)
            {
                index++;
            }

            else
            {
                if (IsLoop)
                {
                    index = 0;
                }
            }
        }
        if (index == 1)
        {
            rotate.y = 0;
            transform.rotation = Quaternion.Euler(rotate);
        }
        if (index == 2)
        {
            rotate.y = -90;
            transform.rotation = Quaternion.Euler(rotate);
        }
        if (index == 3)
        {
            rotate.y = 180;
            transform.rotation = Quaternion.Euler(rotate);
        }
        if (index == 4)
        {
            rotate.y = 90;
            
            transform.rotation = Quaternion.Euler(rotate);
        }


    }
}
