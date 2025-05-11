using UnityEngine;

namespace Puzzle.Portal_Game {
  public class PortalControl : MonoBehaviour {
    [SerializeField] private GameObject leftPortal;
    [SerializeField] private GameObject rightPortal;

    [SerializeField] private Transform leftPortalSpawnPoint;
    [SerializeField] private Transform rightPortalSpawnPoint;

    [SerializeField] private Collider2D leftPortalCollider;
    [SerializeField] private Collider2D rightPortalCollider;

    [SerializeField] private GameObject clone;

    [SerializeField] private bool isPortalOn;
    [SerializeField] private int portalColor;


    private void Start() {
      leftPortalCollider = leftPortal.GetComponent<BoxCollider2D>();
      rightPortalCollider = rightPortal.GetComponent<BoxCollider2D>();

      if (isPortalOn)
        EnablePortals();
      else {
        DisablePortals();
        Field.OnWireConnected += AcceptPortalEnable;
      }
    }

    private void AcceptPortalEnable(int color) {
      if (color == portalColor)
        EnablePortals();
    }

    private void EnablePortals() {
      EnableColliders();
      leftPortal.GetComponent<Portal>().SwitchTransparency(false);
      rightPortal.GetComponent<Portal>().SwitchTransparency(false);
    }

    private void DisablePortals() {
      DisableColliders();
      leftPortal.GetComponent<Portal>().SwitchTransparency(true);
      rightPortal.GetComponent<Portal>().SwitchTransparency(true);
    }

    public void SpawnClone(string portal) {
      if (portal == "left") {
        var cloneInstance = Instantiate(clone, leftPortalSpawnPoint.position, Quaternion.identity);
        cloneInstance.gameObject.name = "clone";
      }
      else if (portal == "right") {
        var cloneInstance = Instantiate(clone, rightPortalSpawnPoint.position, Quaternion.identity);
        cloneInstance.gameObject.name = "clone";
      }
    }

    public void DisableCollider(string colliderToDisable) {
      if (colliderToDisable == "left") {
        leftPortalCollider.enabled = false;
      }
      else if (colliderToDisable == "right") {
        rightPortalCollider.enabled = false;
      }
    }

    public void DisableColliders() {
      DisableCollider("right");
      DisableCollider("left");
    }

    public void EnableColliders() {
      leftPortalCollider.enabled = true;
      rightPortalCollider.enabled = true;
    }
  }
}
