using Craft.Managers;
using UnityEngine;

namespace Craft.Decor {
  public class PlayMusicOnStartScript : MonoBehaviour {
    [SerializeField] private AudioClip musicClip;
    private void Start() {
      MusicManager.Instance.PlayMusic(musicClip);
    }
  }
}
