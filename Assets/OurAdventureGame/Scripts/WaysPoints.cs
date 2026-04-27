using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaysPoints : MonoBehaviour
{
    public List<GameObject> points;
    public float speed = 2;
    int index = 0;
    public bool IsLoop = true; 
    void Start()
    {
        
    }

   
    void Update()
    {
        Vector3 destinatioon = points[index].transform.position;
        Vector3 NewPos = Vector3.MoveTowards(transform .position, destinatioon, speed * Time.deltaTime);
        transform.position = NewPos;
        float distance = Vector3.Distance(transform .position, destinatioon);
        Vector3 rotate = transform.eulerAngles;

        if (distance <= 0.05)
        {
            if(index < points.Count - 1)
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
    }
}
