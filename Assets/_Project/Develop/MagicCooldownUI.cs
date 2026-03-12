using UnityEngine;
using TMPro; // Обязательно используем TextMeshPro для качественного текста

public class MagicCooldownUI : MonoBehaviour
{
    [SerializeField] private GameObject energyShieldActive; // Иконка
    [SerializeField] private TextMeshProUGUI cooldownText;  // Текст таймера

    // Внедряем зависимость от логики боя
    public void Construct(PlayerCombat combat)
    {
        // Подписываемся на события
        combat.OnMagicCooldownStarted += HandleCooldownStarted;
        combat.OnMagicCooldownTick += HandleCooldownTick;
        combat.OnMagicCooldownFinished += HandleCooldownFinished;

        // Инициализируем стартовое состояние (Магия готова)
        HandleCooldownFinished();
    }

    private void HandleCooldownStarted()
    {
        energyShieldActive.SetActive(false); // Отключаем иконку
        cooldownText.gameObject.SetActive(true); // Включаем текст
    }

    private void HandleCooldownTick(float remainingTime)
    {
        // Округляем время до 1 знака после запятой (например: 4.5)
        cooldownText.text = remainingTime.ToString("F1");
    }

    private void HandleCooldownFinished()
    {
        energyShieldActive.SetActive(true); // Включаем иконку
        cooldownText.text = "";             // Очищаем текст
        cooldownText.gameObject.SetActive(false); // Прячем текст
    }
}