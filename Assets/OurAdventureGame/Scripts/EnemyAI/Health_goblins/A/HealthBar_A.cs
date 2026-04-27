using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar_A : MonoBehaviour
{
    private Slider _healthbarSlider;
    [Header("Damage Effect Settings")]
    [SerializeField] private Slider _damageEffectSlider_A;
    [SerializeField] private float damageSliderAnimationTime_A;
    [SerializeField] private float damageSliderDelayTime_A;

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

    public void UpdateHealthBar_A(int maxHealth, int currentHealth)
    {
        _healthbarSlider.maxValue = maxHealth;
        _healthbarSlider.minValue = 0;
        _healthbarSlider.value = currentHealth;

        _damageEffectSlider_A.maxValue = maxHealth;
        DamageEffectAnimation_A(currentHealth);

    }

    private void DamageEffectAnimation_A(int targetValue)
    {
        _damageEffectSlider_A.DOKill();
        _damageEffectSlider_A.DOValue(targetValue, damageSliderAnimationTime_A).SetDelay(damageSliderDelayTime_A);
    }
}
