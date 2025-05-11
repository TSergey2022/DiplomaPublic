using System;
using System.Collections.Generic;
using Craft.Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Craft.UI {
  public class ButtonScript : MonoBehaviour {
    private static Dictionary<string, Action> _actions;

    [SerializeField] private string actionKey;

    public static IEnumerable<string> ActionKeys => _actions == null ? Array.Empty<string>() : _actions.Keys;

    private void Awake() {
      EnsureActionsInitialized();
    }

    private void Start() {
      if (_actions.TryGetValue(actionKey, out var action)) {
        GetComponent<Button>().onClick.AddListener(() => action.Invoke());
      }
      else {
        Debug.LogWarning($"[ButtonScript] Action '{actionKey}' not found on button '{name}'");
      }
    }

    private static void EnsureActionsInitialized() {
      if (_actions != null) return;

      _actions = new Dictionary<string, Action> {
        [nameof(StartNewGame)] = StartNewGame,
        [nameof(Continue)] = Continue,
        [nameof(Quit)] = Quit,
        [nameof(Resume)] = Resume,
        [nameof(QuitToMainMenu)] = QuitToMainMenu,
        [nameof(GoToAsteroid1)] = GoToAsteroid1,
        [nameof(GoToAsteroid2)] = GoToAsteroid2,
        [nameof(GoToAsteroid3)] = GoToAsteroid3,
        [nameof(GoToStation)] = GoToStation,
      };
    }

    private static void StartNewGame() {
      GameManager.Instance.StartNewGame();
    }

    private static void Continue() {
      GameManager.Instance.ContinueGame();
    }

    private static void Quit() {
      SaveManager.Instance.SaveToDisk();
#if UNITY_EDITOR
      EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }

    private static void Resume() {
      GameManager.Instance.ResumeGame();
    }

    private static void QuitToMainMenu() {
      SaveManager.Instance.SaveToDisk();
      GameManager.Instance.ResumeGame();
      GameManager.Instance.GoToMainMenu();
    }

    private static void GoToAsteroid1() {
      GameManager.Instance.GoToAsteroid(1);
    }

    private static void GoToAsteroid2() {
      GameManager.Instance.GoToAsteroid(2);
    }

    private static void GoToAsteroid3() {
      GameManager.Instance.GoToAsteroid(3);
    }
    
    private static void GoToStation() {
      GameManager.Instance.GoToStation();
    }
  }
}
