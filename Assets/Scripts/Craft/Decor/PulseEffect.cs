using UnityEngine;

namespace Craft.Decor {
  public class PulseEffect : MonoBehaviour {
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float scaleAmount = 0.05f;

    private Vector3 originalScale;

    private void Start() {
      originalScale = transform.localScale;
    }

    private void Update() {
      var deltaScale = scaleAmount + Mathf.Sin(Time.time * pulseSpeed) * scaleAmount;
      transform.localScale = originalScale + Vector3.one * deltaScale;
    }
  }
}
