using UnityEngine;

namespace Puzzle {
  public class StartWires : MonoBehaviour {
    [SerializeField] private GameObject wires;
    private bool playerNear;

    private void OnTriggerStay2D(Collider2D other) {
      playerNear = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
      playerNear = false;
    }

    private void Update() {
      if (Input.GetKeyDown(KeyCode.E))
        wires.SetActive(true);
    }
  }
}
