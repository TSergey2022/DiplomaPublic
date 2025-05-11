using UnityEngine;

namespace Puzzle {
  [CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/DialogueData")]
  public class DialogueData : ScriptableObject {
    [TextArea(3, 5)] public string[] dialogueLines;
  }
}
