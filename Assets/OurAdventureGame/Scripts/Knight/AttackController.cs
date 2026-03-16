using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    [SerializeField] private ParticleSystem groundBreakEffect;
    [SerializeField] private Slider _reloadAttack;
    [SerializeField] private GameObject _attackSpriteBG;
    [SerializeField] private GameObject _attackSpriteIcon;
    [SerializeField] private float _delayTime = 5f;

    [SerializeField] private Animator _animator;
    const string ATTACK2 = "attack2";

    private bool isCoroutineRunning = false;

    private void Start()
    {
        _reloadAttack.value = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && (isCoroutineRunning == false))
        {
            StartCoroutine(DelayTime());
        }
    }


    private IEnumerator DelayTime()
    {
        isCoroutineRunning = true;

        _animator.SetTrigger(ATTACK2);

        StartCoroutine(ReloadSlider());
        yield return new WaitForSeconds(_delayTime);

        isCoroutineRunning = false;
    }
     

    private IEnumerator ReloadSlider()
    {
        float elapsedTime = 0f;
        _attackSpriteBG.SetActive(false);
        _attackSpriteIcon.SetActive(false);
        while (elapsedTime < _delayTime)
        {
            _reloadAttack.value = Mathf.Lerp(0f, 1f, elapsedTime / _delayTime);

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        _attackSpriteBG.SetActive(true);
        _attackSpriteIcon.SetActive(true);

        _reloadAttack.value = 1f;

    }

    public void PlayGroundBreakEffect()
    {
        if (groundBreakEffect != null)
        {
            groundBreakEffect.Play();
        }
    }
}

