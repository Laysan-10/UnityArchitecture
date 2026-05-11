using System;
using UnityEngine;

[DisallowMultipleComponent]
public class BossElementController : MonoBehaviour
{
    public enum BossElementType
    {
        Storm,
        Frost,
        Fire,
        Lightning
    }

    public enum BossWeaponMode
    {
        Melee
    }

    [Header("Debug")]
    [SerializeField] private BossElementType currentElement;

    [Header("Glow Names")]
    [SerializeField] private string stormGlowName = "StormMeshGlow";
    [SerializeField] private string frostGlowName = "FrostMeshGlow";
    [SerializeField] private string fireGlowName = "FireMeshGlow";
    [SerializeField] private string lightningGlowName = "LightningMeshGlow";

    [Header("Melee Burst")]
    [SerializeField] private int meleeBurstCount = 12;
    [SerializeField] private int attacksPerElementChange = 2;
    [SerializeField] private float stormMagicRatio = 0.35f;
    [SerializeField] private float frostMagicRatio = 0.25f;
    [SerializeField] private float fireMagicRatio = 0.5f;
    [SerializeField] private float lightningMagicRatio = 0.3f;

    private ParticleSystem _stormGlow;
    private ParticleSystem _frostGlow;
    private ParticleSystem _fireGlow;
    private ParticleSystem _lightningGlow;
    private ParticleSystem _activeGlow;
    private int _attacksSinceLastElementChange;

    public BossElementType CurrentElement => currentElement;

    private void Awake()
    {
        CacheGlows();
        ApplyVisualState();
    }

    public void Initialize(BossElementType element, BossWeaponMode mode)
    {
        currentElement = element;
        _attacksSinceLastElementChange = 0;
        CacheGlows();
        ApplyVisualState();
    }

    public void ModifyDamage(bool isPowerAttack, ref float physicalDamage, ref float magicDamage)
    {
        float powerMultiplier = isPowerAttack ? 1.25f : 1f;
        float elementalRatio = currentElement switch
        {
            BossElementType.Storm => stormMagicRatio,
            BossElementType.Frost => frostMagicRatio,
            BossElementType.Fire => fireMagicRatio,
            BossElementType.Lightning => lightningMagicRatio,
            _ => 0f
        };

        magicDamage += physicalDamage * elementalRatio * powerMultiplier;
    }

    public void PlayWeaponAttackEffect()
    {
        if (_activeGlow == null)
        {
            return;
        }

        _activeGlow.Emit(meleeBurstCount);
    }

    public void RegisterAttack()
    {
        _attacksSinceLastElementChange++;

        if (_attacksSinceLastElementChange < Mathf.Max(1, attacksPerElementChange))
        {
            return;
        }

        _attacksSinceLastElementChange = 0;
        SwitchToNextRandomElement();
    }

    private void CacheGlows()
    {
        _stormGlow = FindGlow(stormGlowName);
        _frostGlow = FindGlow(frostGlowName);
        _fireGlow = FindGlow(fireGlowName);
        _lightningGlow = FindGlow(lightningGlowName);
    }

    private void ApplyVisualState()
    {
        SetGlowActive(_stormGlow, currentElement == BossElementType.Storm);
        SetGlowActive(_frostGlow, currentElement == BossElementType.Frost);
        SetGlowActive(_fireGlow, currentElement == BossElementType.Fire);
        SetGlowActive(_lightningGlow, currentElement == BossElementType.Lightning);

        _activeGlow = currentElement switch
        {
            BossElementType.Storm => _stormGlow,
            BossElementType.Frost => _frostGlow,
            BossElementType.Fire => _fireGlow,
            BossElementType.Lightning => _lightningGlow,
            _ => null
        };
    }

    private void SwitchToNextRandomElement()
    {
        Array values = Enum.GetValues(typeof(BossElementType));
        if (values.Length <= 1)
        {
            return;
        }

        BossElementType nextElement = currentElement;
        while (nextElement == currentElement)
        {
            nextElement = (BossElementType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }

        currentElement = nextElement;
        ApplyVisualState();
    }

    private ParticleSystem FindGlow(string glowName)
    {
        if (string.IsNullOrWhiteSpace(glowName))
        {
            return null;
        }

        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (string.Equals(child.name, glowName, StringComparison.OrdinalIgnoreCase))
            {
                return child.GetComponent<ParticleSystem>();
            }
        }

        return null;
    }

    private static void SetGlowActive(ParticleSystem glow, bool isActive)
    {
        if (glow == null)
        {
            return;
        }

        glow.gameObject.SetActive(isActive);

        if (isActive)
        {
            glow.Play(true);
            return;
        }

        glow.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
