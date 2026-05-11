using UnityEngine;

[DisallowMultipleComponent]
public class RareVisualController : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] rareEffects;

    private bool _isRareActive;

    private void Awake()
    {
        ApplyVisualState(false);
    }

    public void SetRareState(bool isRare)
    {
        if (_isRareActive == isRare)
        {
            return;
        }

        _isRareActive = isRare;
        ApplyVisualState(isRare);
    }

    private void ApplyVisualState(bool isRare)
    {
        if (rareEffects == null)
        {
            return;
        }

        foreach (ParticleSystem effect in rareEffects)
        {
            if (effect == null)
            {
                continue;
            }

            effect.gameObject.SetActive(isRare);

            if (isRare)
            {
                effect.Play(true);
                continue;
            }

            effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
