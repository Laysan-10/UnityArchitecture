using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBarUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider healthbarSlider;     
    [SerializeField] private Slider damageEffectSlider;   

    [Header("Settings")]
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private float delayTime = 0.5f;
    
    // НОВАЯ НАСТРОЙКА: Прятать ли полоску при смерти?
    [Tooltip("Включите для мобов. Выключите для игрока.")]
    [SerializeField] private bool hideOnDeath = false;

    private HealthCore _healthCore;

    public void Construct(HealthCore core)
    {
        _healthCore = core;
        
        // Подписываемся на события
        _healthCore.OnHealthChanged += HandleHealthChanged;
        _healthCore.OnDeath += HandleDeath; // Подписка на смерть

        SetMaxHealth(core.MaxHealth);
        UpdateVisuals(core.MaxHealth, core.MaxHealth, false);
    }

    private void SetMaxHealth(float maxHealth)
    {
        healthbarSlider.maxValue = maxHealth;
        damageEffectSlider.maxValue = maxHealth;
    }

    private void HandleHealthChanged(float current, float max)
    {
        // Debug.Log($"UI: Здоровье изменилось! Текущее: {current}, Макс: {max}");
        UpdateVisuals(current, max, true);
    }

    private void UpdateVisuals(float current, float max, bool animate)
    {
        healthbarSlider.value = current;

        if (animate)
        {
            damageEffectSlider.DOKill();
            damageEffectSlider.DOValue(current, animationTime)
                .SetDelay(delayTime)
                .SetEase(Ease.OutQuad);
        }
        else
        {
            damageEffectSlider.value = current;
        }
    }

    // НОВЫЙ МЕТОД: Обработка смерти
    private void HandleDeath()
    {
        if (hideOnDeath)
        {
            // Отключаем родительский объект (Canvas), чтобы скрыть всё
            gameObject.SetActive(false); 
        }
    }

    private void OnDestroy()
    {
        // Отписываемся от событий
        if (_healthCore != null)
        {
            _healthCore.OnHealthChanged -= HandleHealthChanged;
            _healthCore.OnDeath -= HandleDeath;
        }
    }
}