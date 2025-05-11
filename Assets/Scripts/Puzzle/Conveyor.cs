using System;
using Puzzle.Portal_Game.Rover;
using UnityEngine;

namespace Puzzle {
  public class Conveyor : ColorBasedActivatableObject {
    public static Action<int, float> OnRoverStepOnConveyor;
    public static Action<int> OnRoverStepOffConveyor;
    [SerializeField] public Vector2 moveDirection = new(1, 0);
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] private bool isDeactivated;


    private void Start() {
      Field.OnWiresSolved += ActivateConveyor;
    }

    private void ActivateConveyor(ButtonColorType incomingColor) {
      if (ActivatableObjectColor == incomingColor)
        isDeactivated = false;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
      if (isDeactivated) return;
      if (moveDirection.x < 0) {
        Debug.Log("hello");
        collision.gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
      }

      if (collision.CompareTag("Rover")) {
        OnRoverStepOnConveyor?.Invoke(collision.GetComponent<MoveRover>().RoverId, moveSpeed * moveDirection.x);
      }
    }

    private void OnTriggerStay2D(Collider2D other) {
      /*   if(!isDeactivated)
      OnRoverStepOnConveyor?.Invoke(other.GetComponent<MoveRover>().RoverId, moveSpeed); */
    }


    private void OnTriggerExit2D(Collider2D collision) {
      if (collision.CompareTag("Rover")) {
        OnRoverStepOffConveyor?.Invoke(collision.GetComponent<MoveRover>().RoverId);
      }
    }
  }
}
