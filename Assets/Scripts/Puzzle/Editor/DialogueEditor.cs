using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Puzzle.Editor {
  public class DialogueNode {
    public string text;
    public List<DialogueNode> choices = new();
    public Rect rect;
  }

  public class DialogueEditor : EditorWindow {
    private string[] jsonFiles;
    private int selectedFileIndex;
    private NPCDialogue dialogueData;
    private string jsonFileContent;


    private List<DialogueNode> nodes = new();
    private DialogueNode selectedNode;

    [MenuItem("Window/Dialogue Editor")]
    public static void ShowWindow() {
      GetWindow<DialogueEditor>("Dialogue Editor");
    }

    private void OnEnable() {
      var dialogueFolderPath = Application.dataPath + "/Resources/Dialogue/";
      if (Directory.Exists(dialogueFolderPath)) {
        jsonFiles = Directory.GetFiles(dialogueFolderPath, "*.json")
          .Select(Path.GetFileNameWithoutExtension)
          .ToArray();
      }
      else {
        jsonFiles = new string[0];
      }
    }

    private void OnGUI() {
      if (jsonFiles.Length == 0) {
        GUILayout.Label("No dialogue files found in Resources/Dialogue");
        return;
      }

      selectedFileIndex = EditorGUILayout.Popup("Select NPC Dialogue", selectedFileIndex, jsonFiles);

      if (GUILayout.Button("Load Selected File")) {
        var selectedFileName = jsonFiles[selectedFileIndex];
        var jsonFilePath = Application.dataPath + "/Resources/Dialogue/" + selectedFileName + ".json";

        if (File.Exists(jsonFilePath)) {
          // Load the selected JSON file
          jsonFileContent = File.ReadAllText(jsonFilePath);
          Debug.Log("Loaded JSON File: " + selectedFileName);
        }
      }

      if (!string.IsNullOrEmpty(jsonFileContent)) {
        GUILayout.Label("JSON Content:", EditorStyles.boldLabel);
        GUILayout.TextArea(jsonFileContent, GUILayout.Height(200));
      }

      if (GUILayout.Button("Open Dialogue File")) {
        var selectedFileName = jsonFiles[selectedFileIndex];
        var jsonFilePath = Application.dataPath + "/Resources/Dialogue/" + selectedFileName + ".json";

        if (File.Exists(jsonFilePath)) {
          jsonFileContent = File.ReadAllText(jsonFilePath);
          Debug.Log("Loaded JSON File: " + selectedFileName);
        }

        dialogueData = JsonUtility.FromJson<NPCDialogue>(jsonFileContent);

        Debug.Log("Loaded dialogue " + dialogueData.dialogue.Count);

        foreach (var node in dialogueData.dialogue) {
          nodes.Add(new DialogueNode { rect = new Rect(10, 10 + (110 * nodes.Count), 200, 100) });
        }
      }


      BeginWindows();
      if (dialogueData != null && dialogueData.dialogue != null) {
        for (var i = 0; i < dialogueData.dialogue.Count; i++) {
          var node = dialogueData.dialogue[i];
          nodes[i].rect = GUI.Window(i, nodes[i].rect, DrawNodeWindow, $"Node {node.id}");
        }

        EndWindows();

        if (GUILayout.Button("Add Node")) {
          var newNode = new DialogueNode { rect = new Rect(10, 10, 200, 100) };
          nodes.Add(newNode);
        }
      }
    }

    private void DrawNodeWindow(int id) {
      var node = nodes[id];
      GUILayout.Label("Dialogue Text:");
      node.text = GUILayout.TextField(node.text);
      if (GUILayout.Button("Add Choice")) {
        node.choices.Add(new DialogueNode
          { rect = new Rect(node.rect.x + 220, node.rect.y + (110 * node.choices.Count), 200, 100) });
      }

      GUI.DragWindow();
    }
  }
}