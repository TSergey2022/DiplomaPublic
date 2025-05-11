using UnityEngine;

namespace Puzzle.Portal_Game.Rover {
  [CreateAssetMenu(fileName = "RoverPositionData", menuName = "Rover/PositionData")]
  public class RoverPositionData : ScriptableObject {
    public Vector2[] initialPositions;
  }
}
