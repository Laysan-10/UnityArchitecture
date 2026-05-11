using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AttackByEnemy : MonoBehaviour
{
    [Header("Damage value")]
    [SerializeField] private int _damageValueS;
    [SerializeField] private int _damageValueS2;
    [SerializeField] float attackRange = 9f;
    [SerializeField] float attackRange2 = 9f;
    [SerializeField] private Animator _animatorEnemy;
    private Transform _targetEnemy;
    private HealthBarEnemy[] _enemies;
    private float _distance;

    private void Start()
    {
        GameObject targetObject = GameObject.FindWithTag("Enemy");
        GameObject animatorObject = GameObject.FindWithTag("Enemy");
        GameObject scriptObject = GameObject.FindWithTag("Enemy");

        _targetEnemy = targetObject.GetComponent<Transform>();
        _animatorEnemy = animatorObject.GetComponent<Animator>();
        _enemies = FindObjectsOfType<HealthBarEnemy>();
    }

    public void Damage_S1()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy != null)
            {
                _distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (_distance <= attackRange)
                {
                    if (enemy.HealthEnemy != 0)
                    {
                        enemy.TakeDamageByEnemy(_damageValueS);
                    }
                }
            }
        }
    }


    public void Damage_S2()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy != null)
            {
                _distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (_distance <= attackRange2)
                {
                    if (enemy.HealthEnemy != 0)
                    {
                        enemy.TakeDamageByEnemy(_damageValueS2);
                    }
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange2);
    }


}
