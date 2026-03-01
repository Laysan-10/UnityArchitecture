using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour, IDamageble
{
    [Header("Health")]
    [SerializeField] private  int maxHealth;
    private int health;

    [SerializeField] private Animator _animator;

    [Header("Attack UI")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _damagePanel;

    [SerializeField] private DeathMenu _deathMenu;


    public int Health // здоровье
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (health <= 0)
            {
                health = 0;
                Die();               
            }
            healthBar.UpdateHealthBar(maxHealth, health);
        }
    }
    [Header("HealthBar")]
    [SerializeField] private HealthBar healthBar;

    public void Die()
    {
        Debug.Log("Рыцарь умер");
        _animator.SetTrigger("Death");
        _deathMenu.SetActivDeathMenu();
    }

    private void Awake()
    {
        tag = "Player";
    }

    private void Start()
    {
        Health = maxHealth;
    }

    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
        ShowPanel();

    }

    private void GetHealth(int value)
    {

    }

    private void ShowPanel()
    {
        _damagePanel.SetActive(true);

        // Проверяем, что у нас есть компонент CanvasGroup
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 1f; // Устанавливаем начальное значение прозрачности

            // Анимация прозрачности с использованием DOTween
            _canvasGroup.DOFade(0f, 1.5f).OnComplete(HidePanel); // OnComplete вызывается после завершения анимации
        }
    }

    private void HidePanel()
    {
        _damagePanel.SetActive(false);
    }
}

