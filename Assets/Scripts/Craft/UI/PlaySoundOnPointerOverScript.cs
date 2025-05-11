using Craft.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Craft.UI {
  public class PlaySoundOnPointerOverScript : MonoBehaviour, IPointerEnterHandler {
    [SerializeField] private AudioClip sound;

    public void OnPointerEnter(PointerEventData eventData) {
      SoundManager.Instance.PlaySound(sound);
    }
  }
}
