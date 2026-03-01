using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarEnemy_A : MonoBehaviour
{
    int pers;
    private CharacterSettings _characterSettings;
    [SerializeField] private Animator _animator;

    [Header("Health")]
    [SerializeField] private int maxHealth_A;
    private int healthEnemy_A = 50;

    [Header("HealthBar")]
    [SerializeField] private HealthBar_A healthBar_A;
    [SerializeField] private GameObject HB_active_A;
    [SerializeField]private dark_magic_ball _dark_magic_ballByPlayer;

    public int HealthEnemy_A
    {
        get
        {
            return healthEnemy_A;
        }
        set
        {
            healthEnemy_A = value;
            if (healthEnemy_A <= 0)
            {
                healthEnemy_A = 0;
                HB_active_A.SetActive(false);
                DieEnemy_A();
            }
            healthBar_A.UpdateHealthBar_A(maxHealth_A, healthEnemy_A);
        }
    }

    private void Start()
    {
        pers = PlayerPrefs.GetInt("CharacterSelected"); // получаем значени по ключи из PlayerPrefs
        HealthEnemy_A = maxHealth_A;
        Debug.Log(HealthEnemy_A + " -- здоровья");
    }

    public void TakeDamageByEnemy_A(int damageValue)
    {
        HealthEnemy_A -= damageValue;
        Debug.Log(HealthEnemy_A + " -- здоровья");

    }

    public void DieEnemy_A()
    {

        Debug.Log("Враг умер");
        _animator.SetTrigger("death");
        GetComponent<Collider>().enabled = false;
        GetComponent<dark_magic_ball>().enabled = false;
        //_dark_magic_ballByPlayer.enabled = false;
        if(pers == 0)
        {
           _characterSettings._dark_magic_ballByPlayer1.enabled = false;
        }
        if(pers == 1)
        {
            _characterSettings._dark_magic_ballByPlayer2.enabled = false;

        }
        DOTween.Kill(transform);

        // Запускаем корутину для затухания врага
        StartCoroutine(FadeOut());
    }


    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
}