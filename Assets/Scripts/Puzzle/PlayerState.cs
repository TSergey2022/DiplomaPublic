using UnityEngine;

namespace Puzzle {
  public class PlayerState : MonoBehaviour {
    public enum DirectionEnum {
      NONE,
      LEFT,
      RIGHT
    }

    public DirectionEnum direction = DirectionEnum.NONE;
  }
}
