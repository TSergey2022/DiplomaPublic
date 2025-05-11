using UnityEngine;
using UnityEngine.Events;

namespace Craft.World {
  public class InteractableScript : MonoBehaviour {
    [SerializeField] private UnityEvent onInteract;

    public void Interact() {
      onInteract.Invoke();
      Debug.Log($"Interacted with ({gameObject.name})");
    }

    // Optional: Add visual feedback when interactable
    private void OnTriggerEnter2D(Collider2D other) {
      if (other.CompareTag("Player")) {
        // Highlight object or show UI prompt
      }
    }

    private void OnTriggerExit2D(Collider2D other) {
      if (other.CompareTag("Player")) {
        // Remove highlight or UI prompt
      }
    }
  }
}
