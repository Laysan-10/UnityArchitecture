using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Hints : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _hint;
    [SerializeField] private float _delay;

    private void Start()
    {
        StartCoroutine(Delay());
    }

    private void ShowHint()
    {
        _hint.SetActive(true);

        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 1f; // ”станавливаем начальное значение прозрачности

            // јнимаци€ прозрачности с использованием DOTween
            _canvasGroup.DOFade(0f, 1.5f).OnComplete(HideHint); // OnComplete вызываетс€ после завершени€ анимации
        }
    }

    private void HideHint()
    {
        _hint.SetActive(false); 
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(_delay);
        ShowHint();
    }
}
