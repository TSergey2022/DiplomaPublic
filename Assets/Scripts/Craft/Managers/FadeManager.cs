using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Craft.Managers {
  public sealed class FadeManager : MonoBehaviour {
    [SerializeField] private float duration = 1.5f;

    public UnityEvent onFadeIn;
    public UnityEvent onFadeOut;

    [SerializeField] private RectTransform rectTransform;
    private Coroutine currentRoutine;

    public static FadeManager Instance { get; private set; }

    private enum FadeState {
      Idle,
      FadingIn,
      FadingOut,
      Waiting
    }

    private FadeState state = FadeState.Idle;
    private bool queuedFadeIn;
    private bool queuedFadeOut;
    private bool currentFromRight = true;


    private void Awake() {
      if (!Instance) {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetCurtainSize(1f, false);
      } else if (Instance != this) {
        Destroy(gameObject);
      }
    }

    public void FadeIn() {
      if (state == FadeState.FadingIn) queuedFadeOut = false;
      if (state == FadeState.FadingIn || state == FadeState.Waiting)
        return;

      if (state == FadeState.FadingOut) {
        queuedFadeIn = true;
        return;
      }

      StartFadeRoutine(false);
    }

    public void FadeOut() {
      if (state == FadeState.FadingOut) queuedFadeIn = false;
      if (state == FadeState.FadingOut || state == FadeState.Waiting)
        return;

      if (state == FadeState.FadingIn) {
        queuedFadeOut = true;
        return;
      }

      StartFadeRoutine(true);
    }

    private void StartFadeRoutine(bool fadeOut) {
      if (currentRoutine != null)
        StopCoroutine(currentRoutine);

      currentRoutine = StartCoroutine(FadeRoutine(fadeOut));
    }

    private IEnumerator FadeRoutine(bool fadeOut) {
      state = fadeOut ? FadeState.FadingOut : FadeState.FadingIn;

      var time = 0f;
      var start = GetCurtainNormalizedWidth(currentFromRight);
      currentFromRight = fadeOut;

      var end = fadeOut ? 1f : 0f;

      while (time < duration) {
        time += Time.deltaTime;
        var t = Mathf.Clamp01(time / duration);
        var easedT = EaseInOutCubic(t);
        var width = Mathf.Lerp(start, end, easedT);
        SetCurtainSize(width, fadeOut);
        yield return null;
      }

      SetCurtainSize(end, fadeOut);
      state = FadeState.Idle;
      currentRoutine = null;

      // Вызываем события
      if (fadeOut)
        onFadeOut?.Invoke();
      else
        onFadeIn?.Invoke();

      // Обрабатываем отложенные вызовы
      if (queuedFadeOut) {
        queuedFadeOut = false;
        StartFadeRoutine(true);
      }
      else if (queuedFadeIn) {
        queuedFadeIn = false;
        StartFadeRoutine(false);
      }
    }

    private void SetCurtainSize(float normalizedWidth, bool fromRight) {
      if (fromRight) {
        rectTransform.anchorMin = new Vector2(1f - normalizedWidth, 0f);
        rectTransform.anchorMax = new Vector2(1f, 1f);
      }
      else {
        rectTransform.anchorMin = new Vector2(0f, 0f);
        rectTransform.anchorMax = new Vector2(normalizedWidth, 1f);
      }

      rectTransform.offsetMin = Vector2.zero;
      rectTransform.offsetMax = Vector2.zero;
    }

    private float GetCurtainNormalizedWidth(bool fromRight) {
      return fromRight
        ? 1f - rectTransform.anchorMin.x
        : rectTransform.anchorMax.x;
    }

    private static float EaseInOutCubic(float t) {
      return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    }
  }
}
