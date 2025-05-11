using System;
using UnityEngine;

namespace Puzzle {
  public class GameController : MonoBehaviour {
    public static GameController instance;

    public bool SideWalkRuleOn = true;

    public InputManager InputManager { get; private set; }

    public event Action<bool> OnResumeOpen;
    public event Action<bool> OnMapOpen;
    [SerializeField] private GameObject pointer;

    private bool TimeIsPaused() => Math.Abs(Time.timeScale) < float.Epsilon;

    private void Awake() {
      if (instance == null) {
        instance = this;
        DontDestroyOnLoad(gameObject);
      }
      else if (instance != this) {
        Destroy(gameObject);
      }

      InputManager = gameObject.AddComponent<InputManager>();
      InputManager.OpenResume += ViewResume;
      InputManager.OpenMap += ViewMap;
    }

    private void Start() {
      DialogueUI.Instance.OpenDialogue += PauseTime;
      DialogueUI.Instance.CloseDialogue += ResumeTime;
    }

    public void ViewResume() {
      Time.timeScale = TimeIsPaused() ? 1 : 0;
      OnResumeOpen?.Invoke(TimeIsPaused());
    }

    public void ViewMap() {
      Time.timeScale = TimeIsPaused() ? 1 : 0;
      OnMapOpen?.Invoke(TimeIsPaused());
    }

    private void PauseTime() {
      Time.timeScale = 0;
    }

    private void ResumeTime() {
      Time.timeScale = 1;
    }
  }
}
