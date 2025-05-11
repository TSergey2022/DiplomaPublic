using System;
using Craft.Managers;
using UnityEngine;

namespace Puzzle {
  public class Door : MonoBehaviour {
    [SerializeField] private int x;
    [SerializeField] private int y;

    public static Action<int, int> OnRoomEntered;
    private bool isInTrigger;
    
#if UNITY_EDITOR
    [HideInInspector]
    public UnityEditor.SceneAsset sceneAsset;
#endif
    
    [SerializeField, HideInInspector]
    private string scenePath;

    private void OnTriggerEnter2D(Collider2D col) {
      isInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D col) {
      isInTrigger = false;
    }

    private void EnterDoor() {
      OnRoomEntered?.Invoke(x, y);
      GameManager.Instance.LoadSceneWithFader(scenePath);
    }


    private void Update() {
      if (isInTrigger && Input.GetKeyDown(KeyCode.E)) {
        EnterDoor();
      }
    }
  }
}
