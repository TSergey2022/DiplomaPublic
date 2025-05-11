using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
  public class DialogueChoice {
    public string text;
    public int nextId;
  }

  public class DialogueNode {
    public int id;
    public string text;
    public List<DialogueChoice> choices;
  }


  public class NPCDialogue {
    public string npcName;
    public List<DialogueNode> dialogue;
  }


  public class DialogueManager : MonoBehaviour {
    public TextAsset jsonFile;
    private NPCDialogue npcDialogue;
    public int currentNodeId;

    private void Start() {
      npcDialogue = JsonUtility.FromJson<NPCDialogue>(jsonFile.text);
      currentNodeId = 0;
    }
  }
}
