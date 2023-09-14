using System.Collections;
using UnityEngine;

public class Menu : MonoBehaviour {

    [Header("Fade Transition")]

    [Tooltip("Duration of the fade in and out")]
    [SerializeField] private float _fadeDuration;
    [Tooltip("The time between ending fade out and starting fade in")]
    [SerializeField] private float _fadeOpenDelay;
    private CanvasGroup _currentUi;

    [Header("Move Transition")]

    [Tooltip("Duration of the move in and out")]
    [SerializeField] private float _moveDuration;
    [Tooltip("The time between ending move out and starting move in")]
    [SerializeField] private float _moveOpenDelay;

    [Header("Cache")]

    private float _floatC;
    private WaitForSeconds _fadeDelayWait;
    private WaitForSeconds _moveDelayWait;

    private void Awake() {
        _fadeDelayWait = new WaitForSeconds(_fadeOpenDelay);
        _moveDelayWait = new WaitForSeconds(_moveOpenDelay);
    }

    public void OpenUIFade(CanvasGroup fadeIn) {
        StartCoroutine(FadeUITransition(_currentUi, fadeIn));
    }

    private IEnumerator FadeUITransition(CanvasGroup fadeOut, CanvasGroup fadeIn) {
        fadeOut.interactable = false;
        fadeOut.blocksRaycasts = false;
        while (fadeOut.alpha > 0) {
            fadeOut.alpha -= Time.deltaTime / _fadeDuration;

            yield return null;
        }
        fadeOut.alpha = 0; //

        yield return _fadeDelayWait;

        while (fadeIn.alpha < 1) {
            fadeIn.alpha += Time.deltaTime / _fadeDuration;

            yield return null;
        }
        fadeIn.alpha = 1; //
        fadeIn.interactable = false;
        fadeIn.blocksRaycasts = false;

    }

    public void OpenUIMove(RectTransform moveIn) {
        StartCoroutine(MoveUITransition(null, moveIn));
    }

    public void CloseUIMove(RectTransform moveOut) {
        StartCoroutine(MoveUITransition(moveOut, null));
    }

    private IEnumerator MoveUITransition(RectTransform moveOut, RectTransform moveIn) {
        if (moveOut != null) {
            _floatC = 1;
            while (_floatC > 0) {
                moveOut.anchoredPosition = Vector2.Lerp(Vector2.one, Vector2.zero, _floatC); //
                _floatC -= Time.deltaTime / _moveDuration;
            }
            moveOut.anchoredPosition = Vector2.zero;
        }


        if (moveIn != null) {
            yield return _moveDelayWait;

            _floatC = 0;
            while (_floatC < 1) {
                moveIn.anchoredPosition = Vector2.Lerp(Vector2.zero, Vector2.one, _floatC); //
                _floatC += Time.deltaTime / _moveDuration;
            }
            moveIn.anchoredPosition = Vector2.one;
        }
    }
}
