using UnityEngine;
using System;

public abstract class Heath : IHealth
{
   public int _current { get; protected set;}
   public int _max { get; protected set;}
   
   public Heath(int max)
   {
       _current = max;
       _max = max;
   }

    public virtual void Reduce(int count)
    {
        _current = _current - count;
    }
 
    public virtual bool isDead()
    {
        return _current <= 0;
    }   
}

