#if UNITY_EDITOR
using Puzzle;
using UnityEditor;
using UnityEngine;

namespace Craft.Editor {
  [CustomEditor(typeof(ExitScript))]
  public class ExitScriptEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      var holder = (ExitScript)target;
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
          Debug.Log(path);
          if (path.EndsWith(".unity")) {
            Undo.RecordObject(holder, "Update Scene Path");
            typeof(ExitScript)
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
#endif
