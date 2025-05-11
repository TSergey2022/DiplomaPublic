using UnityEngine;

namespace Puzzle {
  public class NPC : MonoBehaviour {
    public string ID;
    public DialogueData dialogueData;
    private bool playerNearby;


    public virtual void Interact() {
      if (dialogueData != null) {
        QuestEvents.OnNPCInteracted?.Invoke(ID);
        DialogueUI.Instance.ShowDialogue(dialogueData.dialogueLines[0]);
      }
    }

    private void EndDialogue() {
      DialogueUI.Instance.HideDialoguePanel();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
      if (collision.CompareTag("Player")) {
        playerNearby = true;
      }
    }

    private void OnTriggerExit2D(Collider2D collision) {
      if (collision.CompareTag("Player")) {
        playerNearby = false;
      }
    }

    private void Update() {
      if (playerNearby && Input.GetKeyDown(KeyCode.E)) {
        Interact();
      }
    }
  }
}
