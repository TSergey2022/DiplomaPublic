using UnityEngine;

namespace Puzzle.Portal_Game.Rover {
  public class RoverController : MonoBehaviour {
    [SerializeField] private GameObject rover;
    public RoverPositionData positionData;
    private Vector2 initialPosition;
    private Vector2 currentPosition;
    public int levelIndex;


    private void Start() {
      SetInitialPosition();
    }

    private void SetInitialPosition() {
      initialPosition = positionData.initialPositions[levelIndex];
    }

    public void RespawnRover() {
      /*currentPosition = initialPosition;
  transform.position = currentPosition;*/
      Instantiate(rover, initialPosition, Quaternion.identity);
    }

    public void OnRoverDestroyed() {
      RespawnRover();
    }
  }
}
