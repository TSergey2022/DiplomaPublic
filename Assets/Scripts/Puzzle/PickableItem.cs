using UnityEngine;

namespace Puzzle {
  public class PickableItem : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
      if (collision.CompareTag("Player")) {
        Destroy(gameObject);
      }
    }
  }
}
