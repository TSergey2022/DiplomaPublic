using UnityEngine;

namespace Puzzle {
  public class QuestGiver : NPC {
    [SerializeField] public Quest quest;

    public override void Interact() {
      if (QuestManager.instance.currentQuest == null) {
        DialogueUI.Instance.GiveQuest(dialogueData.dialogueLines[0], AcceptQuest, RejectQuest);
      }
    }

    private void AcceptQuest() {
      QuestManager.instance.AcceptQuest(quest);
    }

    private void RejectQuest() {
      Debug.Log("ok bro");
    }
  }
}
