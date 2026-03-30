using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar_S : MonoBehaviour
{
    private Slider _healthbarSlider;
    [Header("Damage Effect Settings")]
    [SerializeField] private Slider _damageEffectSlider;
    [SerializeField] private float damageSliderAnimationTime;
    [SerializeField] private float damageSliderDelayTime;

    private Quaternion _startRotation;

    private void Awake()
    {
        _healthbarSlider = GetComponent<Slider>();
        _startRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = _startRotation;
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
