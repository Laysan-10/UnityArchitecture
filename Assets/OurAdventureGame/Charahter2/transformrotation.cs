using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transformrotation : MonoBehaviour
{
    private bool check;
    [SerializeField]  private Animator animator;
    const string Left = "left";
    const string Right = "right";
    
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D) && check)
        {
            transform.Rotate(0, -180, 0);
            animator.SetBool(Right, true);
            check = false;
        }
        if (Input.GetKey(KeyCode.A) && check != true)
        {
            transform.Rotate(0, 180, 0);
            animator.SetBool(Left, true);
            check = true;
        }
    }
}
