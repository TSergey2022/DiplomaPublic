using Craft.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Puzzle {
  public class PuzzleExit : MonoBehaviour {
    public string PuzzleID;

    private void OnTriggerEnter2D(Collider2D col) {
      QuestEvents.OnPuzzleCompleted?.Invoke(PuzzleID);
      GameManager.Instance.LoadSceneWithFader("Docks");
    }
  }
}
