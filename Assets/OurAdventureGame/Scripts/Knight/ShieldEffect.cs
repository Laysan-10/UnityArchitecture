using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _magic_shield_blue;
    [SerializeField] private BoxCollider[] _colliders;
    [SerializeField] private Slider _shieldSlider;
    [SerializeField] private GameObject _shieldSprite;
    [SerializeField] private float _delayTime = 4f;
    [SerializeField] private float _playTime = 7f;

    private bool isCoroutineRunning = false;

    private void Start()
    {
        _shieldSlider.value = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && (!isCoroutineRunning))
            StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        isCoroutineRunning = true;

        // �������� �������� Slider'�
        StartCoroutine(AnimateSlider());

        PlayMagicShield();
        foreach (var item in _colliders)
        {
            item.enabled = true;
        }

        yield return new WaitForSeconds(_playTime);

        StopMagicShield();
        foreach (var item in _colliders)
        {
            item.enabled = false;
        }

        yield return new WaitForSeconds(_delayTime);
        _shieldSlider.value = 1f;

        isCoroutineRunning = false;
    }

    private IEnumerator AnimateSlider()
    {
        float elapsedTime = 0f;
        _shieldSprite.SetActive(false);
        while (elapsedTime < (_playTime + _delayTime))
        {
            _shieldSlider.value = Mathf.Lerp(0f, 1f, elapsedTime / (_playTime + _delayTime));

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        _shieldSprite.SetActive(true);

        _shieldSlider.value = 1f;
    }

    public void PlayMagicShield()
    {
        foreach (var part in _magic_shield_blue)
        {
            part.Play();
        }
    }

    public void StopMagicShield()
    {
        foreach (var part in _magic_shield_blue)
        {
            part.Stop();
        }
    }
}
