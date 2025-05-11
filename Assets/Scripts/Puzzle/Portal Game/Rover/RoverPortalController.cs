using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle.Portal_Game.Rover {
  public class RoverPortalController : MonoBehaviour {
    [SerializeField] private GameObject enterPortal;
    [SerializeField] private List<GameObject> exitPortals;

    [SerializeField] private Transform enterPortalSpawnPoint;
    [SerializeField] private List<Transform> exitPortalSpawnPoints;

    [SerializeField] private Collider2D enterPortalCollider;
    [SerializeField] private List<Collider2D> exitPortalColliders;
    [SerializeField] private GameObject roverPrefab;

    private List<GameObject> activeClones = new();
    private HashSet<GameObject> clonesAtExit = new();
    private bool isRecombining;

    [SerializeField] private int portalColor;

    [SerializeField] private bool isPortalOn;

    public Action OnClonesRecombined;

    private void Start() {
      enterPortalCollider = enterPortal.GetComponent<Collider2D>();
      foreach (var portal in exitPortals) {
        exitPortalColliders.Add(portal.GetComponent<Collider2D>());
      }

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
      enterPortal.GetComponent<RoverPortal>().SwitchTransparency(false);
      enterPortalCollider.enabled = true;
      foreach (var exitPortal in exitPortals) {
        exitPortal.GetComponent<RoverPortal>().SwitchTransparency(false);
      }

      foreach (var exitPortalCollider in exitPortalColliders) {
        exitPortalCollider.enabled = true;
      }
    }

    private void DisablePortals() {
      enterPortal.GetComponent<RoverPortal>().SwitchTransparency(true);
      enterPortalCollider.enabled = false;
      foreach (var exitPortal in exitPortals) {
        exitPortal.GetComponent<RoverPortal>().SwitchTransparency(true);
      }

      foreach (var exitPortalCollider in exitPortalColliders) {
        exitPortalCollider.enabled = false;
      }
    }


    public void SpawnClonesAtExit(Vector3 scale, int roverScale, int parentId) {
      foreach (var spawnPoint in exitPortalSpawnPoints) {
        var clone = Instantiate(roverPrefab, spawnPoint.position, Quaternion.identity);
        clone.transform.localScale = scale / exitPortals.Count;
        clone.GetComponent<MoveRover>().roverScale = roverScale + 1;

        var cloneId = RoverManager.Instance.RegisterRover(clone, parentId);
        clone.GetComponent<MoveRover>().RoverId = cloneId;

        activeClones.Add(clone);
      }

      clonesAtExit.Clear();
      Debug.Log($"Spawned {activeClones.Count} clones.");
    }

    public void CloneEnteredExitPortal(GameObject clone) {
      if (isRecombining || clonesAtExit.Contains(clone))
        return;

      clonesAtExit.Add(clone);
      Debug.Log($"Clone entered exit portal: {clone.name}. Count at exit: {clonesAtExit.Count}");

      if (clonesAtExit.Count == activeClones.Count) {
        TryRecombineClones();
      }
    }

    public void CloneExitedExitPortal(GameObject clone) {
      if (clonesAtExit.Contains(clone)) {
        clonesAtExit.Remove(clone);
        Debug.Log($"Clone exited exit portal: {clone.name}. Remaining: {clonesAtExit.Count}");
      }
    }

    private void TryRecombineClones() {
      if (isRecombining || clonesAtExit.Count < activeClones.Count)
        return;

      isRecombining = true;

      var combinedScale = Vector3.zero;
      foreach (var clone in activeClones) {
        combinedScale += clone.transform.localScale;
        Destroy(clone);
      }

      activeClones.Clear();

      Debug.Log("Recombining clones into one rover.");

      var recombinedRover = Instantiate(roverPrefab, enterPortalSpawnPoint.position, Quaternion.identity);
      recombinedRover.transform.localScale = combinedScale;
      recombinedRover.GetComponent<MoveRover>().roverScale = 1;

      var cloneId = RoverManager.Instance.RegisterRover(recombinedRover);
      recombinedRover.GetComponent<MoveRover>().RoverId = cloneId;

      OnClonesRecombined?.Invoke();
      EnableEnterPortal();

      isRecombining = false;
    }

    private void EnableEnterPortal() {
      enterPortalCollider.enabled = true;
    }
  }
}
