using UnityEngine;
using TMPro;

public class MagicCooldownUI : MonoBehaviour
{
    [SerializeField] private GameObject energyShieldActive; 
    [SerializeField] private TextMeshProUGUI cooldownText; 

    public void Construct(PlayerCombat combat)
    {
        combat.OnMagicCooldownStarted += HandleCooldownStarted;
        combat.OnMagicCooldownTick += HandleCooldownTick;
        combat.OnMagicCooldownFinished += HandleCooldownFinished;

        HandleCooldownFinished();
    }

    private void HandleCooldownStarted()
    {
        energyShieldActive.SetActive(false);
        cooldownText.gameObject.SetActive(true); 
    }

    private void HandleCooldownTick(float remainingTime)
    {
        cooldownText.text = remainingTime.ToString("F1");
    }

    private void HandleCooldownFinished()
    {
        energyShieldActive.SetActive(true); 
        cooldownText.text = "";
        cooldownText.gameObject.SetActive(false);
    }
}