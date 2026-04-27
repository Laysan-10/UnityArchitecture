using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJustSimpleMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    [SerializeField] float Speed = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void Move()
    {
        float Horisontal1 = Input.GetAxis("Horizontal");
        float Vertival1 = Input.GetAxis("Vertical");
        Vector3 movemwnt = new Vector3(Horisontal1, 0.0f, Vertival1);
        rb.AddForce(movemwnt * Speed);

    }
}
