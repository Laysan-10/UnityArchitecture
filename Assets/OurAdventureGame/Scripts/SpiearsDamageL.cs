using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiearsDamageL : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private int _damageValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spears"))
        {
            _player.TakeDamage(_damageValue);
        }
    }
}
