using System.Linq;
using Craft.UI;
using UnityEditor;

namespace Craft.Editor {
  [CustomEditor(typeof(ButtonScript))]
  public class ButtonScriptEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      var keys = ButtonScript.ActionKeys.ToList();
      if (keys.Count == 0) {
        EditorGUILayout.HelpBox("Нет зарегистрированных действий. Запусти сцену хотя бы один раз для инициализации.",
          MessageType.Warning);
        DrawDefaultInspector();
        return;
      }

      var currentAction = serializedObject.FindProperty("actionKey").stringValue;

      // if (!keys.Contains(currentAction)) {
      //   currentAction = string.IsNullOrEmpty(currentAction) ? keys[0] : currentAction;
      // }

      var selectedIndex = keys.IndexOf(currentAction);
      selectedIndex = EditorGUILayout.Popup("Action", selectedIndex, keys.ToArray());
      serializedObject.FindProperty("actionKey").stringValue = keys[selectedIndex];

      serializedObject.ApplyModifiedProperties();
    }
  }
}
