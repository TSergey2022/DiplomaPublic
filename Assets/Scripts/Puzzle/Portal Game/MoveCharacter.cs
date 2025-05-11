using System;
using UnityEngine;

namespace Puzzle.Portal_Game {
  public class MoveCharacter : MonoBehaviour, IControllable {
    private Rigidbody2D rb;
    private PlayerState _playerState;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float dirX;
    private bool isControlled = true;

    public void SetControl(bool isActive) {
      isControlled = isActive;
    }

    private void Start() {
      rb = GetComponent<Rigidbody2D>();
      _playerState = FindFirstObjectByType<PlayerState>();
    }

    private void Update() {
      if (!isControlled) return;
      if (GameController.instance.SideWalkRuleOn) {
        if (_playerState.direction == PlayerState.DirectionEnum.LEFT) {
          dirX = -Math.Abs(Input.GetAxisRaw("Horizontal") * moveSpeed);
        }
        else if (_playerState.direction == PlayerState.DirectionEnum.RIGHT) {
          dirX = Math.Abs(Input.GetAxisRaw("Horizontal") * moveSpeed);
        }
        else {
          dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        }
      }
      else
        dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;
    }

    private void FixedUpdate() {
#if UNITY_6000
      rb.linearVelocity = new Vector2(dirX, 0f);
#else
      rb.velocity = new Vector2(dirX, 0f);
#endif
    }
  }
}
