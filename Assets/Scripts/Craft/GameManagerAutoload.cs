#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Craft {
  public static class GameManagerAutoload {
    private static GameObject _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void SpawnGlobalPrefab() {
      if (_instance != null) return;

#if UNITY_EDITOR
      var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/GameManager.prefab");
#else
      var prefab = Resources.Load<GameObject>("GameManager");
#endif
      if (prefab != null) {
        _instance = Object.Instantiate(prefab);
      }
      else {
        Debug.LogWarning("GlobalPrefab not found in Resources!");
      }
    }
  }
}
