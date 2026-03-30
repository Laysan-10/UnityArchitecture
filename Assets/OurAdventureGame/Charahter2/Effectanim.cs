using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effectanim : MonoBehaviour
{
    public ParticleSystem groundBreakEffect1;
    public void Attack()
    {
        if (groundBreakEffect1 != null )
        {
            groundBreakEffect1.Play();
        }
       
       


    }
    public void stopAttack()
    {
       groundBreakEffect1.Stop(); 



    }

}
