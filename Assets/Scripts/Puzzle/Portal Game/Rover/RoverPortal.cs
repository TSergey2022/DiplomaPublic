using UnityEngine;

namespace Puzzle.Portal_Game.Rover {
  public class RoverPortal : MonoBehaviour {
    [SerializeField] private RoverPortalController portalController;

    [SerializeField] private SpriteRenderer rightHalf;
    [SerializeField] private SpriteRenderer leftHalf;

    private void Start() {
      portalController = GetComponentInParent<RoverPortalController>();
    }

    public void SwitchTransparency(bool isTransparent) {
      if (isTransparent) {
        var newColor = new Color(rightHalf.color.r, rightHalf.color.g, rightHalf.color.b, 0.5f);
        leftHalf.color = newColor;
        rightHalf.color = newColor;
      }
      else {
        var newColor = new Color(rightHalf.color.r, rightHalf.color.g, rightHalf.color.b, 1f);
        leftHalf.color = newColor;
        rightHalf.color = newColor;
      }
    }

    private void OnTriggerEnter2D(Collider2D col) {
      if (!col.CompareTag("Rover"))
        return;

      var rover = col.GetComponent<MoveRover>();

      if (gameObject.name == "EnterPortal") {
        portalController.SpawnClonesAtExit(col.transform.localScale, rover.roverScale, rover.ParentId);
        RoverManager.Instance.DeregisterRover(rover.RoverId);
        Destroy(col.gameObject);
      }
      else if (gameObject.name == "ExitPortal") {
        portalController.CloneEnteredExitPortal(col.gameObject);
      }
    }

    private void OnTriggerExit2D(Collider2D other) {
      if (!other.CompareTag("Rover"))
        return;

      if (gameObject.name == "ExitPortal") {
        portalController.CloneExitedExitPortal(other.gameObject);
      }
    }
  }
}
