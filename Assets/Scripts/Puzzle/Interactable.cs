using UnityEngine;

namespace Puzzle {
  public class Interactable : MonoBehaviour {
    public string ObjectID;
    public float interactionRange = 3f;
    private bool isPlayerInRange;

    private void Update() {
      if (isPlayerInRange && Input.GetKeyDown(KeyCode.E)) {
        Interact();
      }
    }

    public void Interact() {
      Debug.Log($"Interacted with object: {ObjectID}");
      QuestEvents.OnObjectInteracted?.Invoke(ObjectID);
    }

    private void OnTriggerEnter2D(Collider2D other) {
      if (other.CompareTag("Player")) {
        isPlayerInRange = true;
      }
    }

    private void OnTriggerExit2D(Collider2D other) {
      if (other.CompareTag("Player")) {
        isPlayerInRange = false;
      }
    }
  }
}
