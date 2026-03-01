using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{ 
    public  int Health {  get; set; }
    public void TakeDamage(int damageValue);
    public  void Die()
    {
    }
}
