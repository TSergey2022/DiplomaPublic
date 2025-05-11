using Craft.Managers;
using UnityEngine;

namespace Puzzle {
  public class ExitScript : MonoBehaviour {
#if UNITY_EDITOR
    public UnityEditor.SceneAsset sceneAsset;
#endif
    
    [SerializeField, HideInInspector]
    private string scenePath;

    
    private void OnTriggerEnter2D(Collider2D col) {
      GameManager.Instance.LoadSceneWithFader(scenePath);
    }
  }
}
