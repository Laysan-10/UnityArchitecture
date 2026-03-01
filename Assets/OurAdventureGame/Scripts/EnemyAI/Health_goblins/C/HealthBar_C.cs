using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar_C : MonoBehaviour
{
    private Slider _healthbarSlider;
    [Header("Damage Effect Settings")]
    [SerializeField] private Slider _damageEffectSlider_C;
    [SerializeField] private float damageSliderAnimationTime_C;
    [SerializeField] private float damageSliderDelayTime_C;

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

    public void UpdateHealthBar_C(int maxHealth, int currentHealth)
    {
        _healthbarSlider.maxValue = maxHealth;
        _healthbarSlider.minValue = 0;
        _healthbarSlider.value = currentHealth;

        _damageEffectSlider_C.maxValue = maxHealth;
        DamageEffectAnimation_C(currentHealth);

    }

    private void DamageEffectAnimation_C(int targetValue)
    {
        _damageEffectSlider_C.DOKill();
        _damageEffectSlider_C.DOValue(targetValue, damageSliderAnimationTime_C).SetDelay(damageSliderDelayTime_C);
    }
}
