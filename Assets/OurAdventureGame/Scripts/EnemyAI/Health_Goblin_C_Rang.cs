using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Goblin_C_Rang : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private Animator _animator;
    private int healthEnemy = 40;

    private void Start()
    {
        Debug.Log(HealthEnemy + "");
    }
    public int HealthEnemy
    {
        get
        {
            return healthEnemy;
        }
        set
        {
            healthEnemy = value;
            if (healthEnemy <= 0)
            {
                healthEnemy = 0;
                DieEnemy();
            }

        }
    }

    public void TakeDamageByEnemy(int damageValue)
    {
        HealthEnemy -= damageValue;
        Debug.Log(HealthEnemy + "");

    }

    public void DieEnemy()
    {

        Debug.Log("Enemy died");
        _animator.SetTrigger("death");
        GetComponent<Collider>().enabled = false;
        //GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        transform.DOScale(Vector3.zero, 2f);

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
