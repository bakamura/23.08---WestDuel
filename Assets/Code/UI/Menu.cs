using System.Collections;
using UnityEngine;

public class Menu : MonoBehaviour {

    [Header("Fade Transition")]

    [Tooltip("Duration of the fade in and out")]
    [SerializeField] private float _fadeDuration;
    [Tooltip("The time between ending fade out and starting fade in")]
    [SerializeField] private float _fadeOpenDelay;
    [SerializeField] private CanvasGroup _currentUi;

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
        fadeOut.alpha = 0;

        yield return _fadeDelayWait;

        while (fadeIn.alpha < 1) {
            fadeIn.alpha += Time.deltaTime / _fadeDuration;

            yield return null;
        }
        fadeIn.alpha = 1;
        fadeIn.interactable = false;
        fadeIn.blocksRaycasts = false;

    }

    public void OpenUIMove(RectTransform moveIn, Vector2 moveInActivePos, Vector2 moveInDeactivePos) {
        StartCoroutine(MoveUITransition(null, moveIn, Vector2.zero, Vector2.zero, moveInActivePos, moveInDeactivePos));
    }

    public void CloseUIMove(RectTransform moveOut, Vector2 moveOutActivePos, Vector2 moveOutDeactivePos) {
        StartCoroutine(MoveUITransition(moveOut, null, moveOutActivePos, moveOutDeactivePos, Vector2.zero, Vector2.zero));
    }

    private IEnumerator MoveUITransition(RectTransform moveOut, RectTransform moveIn, Vector2 moveOutActivePos, Vector2 moveOutDeactivePos, Vector2 moveInActivePos, Vector2 moveInDeactivePos) {
        if (moveOut != null) {
            _floatC = 1;
            while (_floatC > 0) {
                moveOut.anchoredPosition = Vector2.Lerp(moveOutActivePos, moveOutDeactivePos, _floatC);
                _floatC -= Time.deltaTime / _moveDuration;
            }
            moveOut.anchoredPosition = moveOutDeactivePos;
        }


        if (moveIn != null) {
            yield return _moveDelayWait;

            _floatC = 0;
            while (_floatC < 1) {
                moveIn.anchoredPosition = Vector2.Lerp(moveInDeactivePos, moveInActivePos, _floatC);
                _floatC += Time.deltaTime / _moveDuration;
            }
            moveIn.anchoredPosition = moveInActivePos;
        }
    }
}
