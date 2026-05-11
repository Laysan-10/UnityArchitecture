using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianAnimator : MonoBehaviour 
{
    

    public Effectanim effect;
    [SerializeField] private Animator _animator; 
    const string RUN = "Running";
    const string JUMP = "Jumping";
    
    [SerializeField] private bool isRun;
    public bool isJump { private get; set; }
    public bool isattack;
    void Start()
    {
       
        _animator = GetComponent<Animator>();
        effect = GetComponent<Effectanim>();
       
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        
        

    }
    protected void Move()
    {
        isRun = false;

        if (Input.GetKey(KeyCode.D))
        {
            _animator.SetBool(RUN, true);
            isRun = true;
        }

        else if (Input.GetKey(KeyCode.A))
        {

            _animator.SetBool(RUN, true);
            isRun = true;
        }

        if (!isRun)
        {
            _animator.SetBool(RUN, false);
        }
        isattack = false;

        if (Input.GetKey(KeyCode.E))
        {
            _animator.SetBool("turnattack", true);

         
            isattack = true;

        }
        if (!isattack)
        {
            _animator.SetBool("turnattack", false);
            effect.stopAttack();
          
        }




    }
   public void Jumping()
    {
        _animator.SetBool("Jumping", true);
    }
    public void JumpingStop()
    {
        _animator.SetBool("Jumping", false);
    }
    public void StopRun()
    {
        _animator.SetBool("Running", false);
        
    }
   
    

}