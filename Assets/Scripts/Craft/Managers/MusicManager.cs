using System.Collections;
using UnityEngine;

namespace Craft.Managers {
  [RequireComponent(typeof(AudioSource))]
  public sealed class MusicManager : MonoBehaviour {
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float restartDelay = 10f;

    private AudioSource audioSource;
    private Coroutine fadeCoroutine;
    private AudioClip currentClip;
    private bool waitingToRestart;

    public static MusicManager Instance { get; private set; }

    private void Awake() {
      if (Instance == null) {
        Instance = this;
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false; // отключаем зацикливание
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;
      }
      else {
        Destroy(gameObject);
      }
    }

    private void Update() {
      if (!audioSource.isPlaying && currentClip && !waitingToRestart) {
        waitingToRestart = true;
        StartCoroutine(RestartAfterDelay());
      }
    }

    public void PlayMusic(AudioClip newClip) {
      if (fadeCoroutine != null)
        StopCoroutine(fadeCoroutine);

      currentClip = newClip;
      waitingToRestart = false;
      fadeCoroutine = StartCoroutine(FadeInNewClip(newClip));
    }

    public void StopMusic() {
      if (fadeCoroutine != null)
        StopCoroutine(fadeCoroutine);

      currentClip = null;
      waitingToRestart = false;
      fadeCoroutine = StartCoroutine(FadeOutAndStop());
    }

    private IEnumerator FadeInNewClip(AudioClip newClip) {
      if (audioSource.isPlaying) {
        yield return StartCoroutine(FadeOut());
      }

      audioSource.clip = newClip;
      audioSource.Play();

      var time = 0f;
      while (time < fadeDuration) {
        audioSource.volume = Mathf.Lerp(0f, 1f, time / fadeDuration);
        time += Time.deltaTime;
        yield return null;
      }

      audioSource.volume = 1f;
    }

    private IEnumerator FadeOutAndStop() {
      yield return StartCoroutine(FadeOut());
      audioSource.Stop();
      audioSource.clip = null;
    }

    private IEnumerator FadeOut() {
      var startVolume = audioSource.volume;
      var time = 0f;

      while (time < fadeDuration) {
        audioSource.volume = Mathf.Lerp(startVolume, 0f, time / fadeDuration);
        time += Time.deltaTime;
        yield return null;
      }

      audioSource.volume = 0f;
    }

    private IEnumerator RestartAfterDelay() {
      yield return new WaitForSeconds(restartDelay);
      PlayMusic(currentClip);
    }
  }
}
