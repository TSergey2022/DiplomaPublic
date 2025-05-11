using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Craft.Managers {
  public sealed class SoundManager : MonoBehaviour {
    [SerializeField] private int poolSize = 5;
    
    [SerializeField] private AudioMixerGroup group;

    private readonly List<AudioSource> audioSources = new();
    private readonly Dictionary<AudioSource, float> lastUsedTime = new();
    
    public static SoundManager Instance { get; private set; }

    private void Awake() {
      if (Instance == null) {
        Instance = this;
        DontDestroyOnLoad(this);
        for (var i = 0; i < poolSize; i++) {
          var source = gameObject.AddComponent<AudioSource>();
          source.outputAudioMixerGroup = group;
          audioSources.Add(source);
          lastUsedTime[source] = -Mathf.Infinity;
        }
      } else {
        Destroy(gameObject);
      }
    }

    public void PlaySound(AudioClip clip) {
      var availableSource = audioSources
        .FirstOrDefault(s => !s.isPlaying);

      if (availableSource == null) {
        availableSource = audioSources
          .OrderBy(s => lastUsedTime[s])
          .First();
        availableSource.Stop();
      }

      availableSource.clip = clip;
      availableSource.Play();
      lastUsedTime[availableSource] = Time.time;
    }

    public void StopSound() {
      foreach (var source in audioSources) {
        source.Stop();
      }
    }
  }
}
