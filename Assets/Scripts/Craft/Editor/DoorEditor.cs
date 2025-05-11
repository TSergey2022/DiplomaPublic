using Puzzle;
using UnityEditor;
using UnityEngine;

namespace Craft.Editor {
  [CustomEditor(typeof(Door))]
  public class DoorEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      DrawDefaultInspector();
      var holder = (Door)target;
      var sceneAssetProp = serializedObject.FindProperty("sceneAsset");
      var scenePathProp = serializedObject.FindProperty("scenePath");

      if (holder.sceneAsset) {
        var path = AssetDatabase.GetAssetPath(holder.sceneAsset);
        if (scenePathProp.stringValue != path) {
          typeof(ExitScript)
            .GetField("scenePath",
              System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(holder, path);
          EditorUtility.SetDirty(holder);
        }
      }
        
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(sceneAssetProp, new GUIContent("Scene Asset"));
      if (EditorGUI.EndChangeCheck()) {
        serializedObject.ApplyModifiedProperties();

        if (holder.sceneAsset) {
          var path = AssetDatabase.GetAssetPath(holder.sceneAsset);
          if (path.EndsWith(".unity")) {
            Undo.RecordObject(holder, "Update Scene Path");
            typeof(Door)
              .GetField("scenePath",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
              ?.SetValue(holder, path);
            EditorUtility.SetDirty(holder);
          }
          else {
            Debug.LogWarning("Selected asset is not a scene.");
          }
        }
      }
      
      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Scene Path (runtime):");
      GUI.enabled = false;
      EditorGUILayout.TextField(scenePathProp.stringValue);
      GUI.enabled = true;
    }
  }
}
