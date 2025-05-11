using System.Collections.Generic;
using Puzzle.Portal_Game.Rover;
using UnityEngine;

namespace Puzzle {
  [System.Serializable]
  public class RoverData {
    public GameObject RoverObject;
    public int RoverId;
    public int ParentId;
    public Vector3 Scale;
  }

  public class RoverManager : MonoBehaviour, IControllable {
    public static RoverManager Instance { get; private set; }
    private Dictionary<int, RoverData> rovers = new();
    private int nextRoverId = 1;

    public bool isControlled;

    public void SetControl(bool isActive) {
      isControlled = isActive;
    }

    private void Awake() {
      if (Instance != null && Instance != this) {
        Destroy(gameObject);
        return;
      }

      Instance = this;
      DontDestroyOnLoad(gameObject);
    }

    public int RegisterRover(GameObject rover, int parentId = 0) {
      var roverId = nextRoverId++;
      var roverData = new RoverData {
        RoverObject = rover,
        RoverId = roverId,
        ParentId = parentId,
        Scale = rover.transform.localScale
      };
      rover.GetComponent<MoveRover>().RoverId = roverId;
      rovers.Add(roverId, roverData);
      return roverId;
    }

    public void DeregisterRover(int roverId) {
      if (rovers.ContainsKey(roverId)) {
        rovers.Remove(roverId);
      }
    }

    public List<int> GetChildRovers(int parentId) {
      var children = new List<int>();
      foreach (var rover in rovers.Values) {
        if (rover.ParentId == parentId) {
          children.Add(rover.RoverId);
        }
      }

      return children;
    }

    public RoverData GetRoverData(int roverId) {
      return rovers.ContainsKey(roverId) ? rovers[roverId] : null;
    }
  }
}
