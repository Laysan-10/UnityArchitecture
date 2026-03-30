using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    private Slider _healthbarSlider;
    [Header("Damage Effect Settings")]
    [SerializeField] private Slider _damageEffectSlider;
    [SerializeField] private float damageSliderAnimationTime;
    [SerializeField] private float damageSliderDelayTime;

    private void Awake()
    {
        _healthbarSlider = GetComponent<Slider>();
    }
    public void UpdateHealthBar(int maxHealth,int currentHealth)
    {
        _healthbarSlider.maxValue = maxHealth;
        _healthbarSlider.minValue = 0;
        _healthbarSlider.value = currentHealth;

        _damageEffectSlider.maxValue = maxHealth;
        DamageEffectAnimation(currentHealth);

    }

    private void DamageEffectAnimation(int targetValue)
    {
        _damageEffectSlider.DOKill();
        _damageEffectSlider.DOValue(targetValue, damageSliderAnimationTime).SetDelay(damageSliderDelayTime);
    }
}
