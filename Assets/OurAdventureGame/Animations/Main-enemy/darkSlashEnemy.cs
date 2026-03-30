using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class darkSlashEnemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public void DarkSlash()
    {
        _particleSystem.Play();
    }

    public void StopSlash()
    {
        _particleSystem.Stop();
    }
}
