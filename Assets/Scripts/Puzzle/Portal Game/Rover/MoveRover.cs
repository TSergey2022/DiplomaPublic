using UnityEngine;

namespace Puzzle.Portal_Game.Rover {
  public class MoveRover : MonoBehaviour {
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float dirX;
    [SerializeField] private RoverController roverController;
    [SerializeField] public int roverScale = 1;
    [SerializeField] public int RoverId; // { get; set; } 
    [SerializeField] public int ParentId; // { get; set; }

    [SerializeField] private bool isOriginal;

    [SerializeField] private bool Conveyored;

    private void Start() {
      rb = GetComponent<Rigidbody2D>();
      roverController = FindFirstObjectByType<RoverController>();
      if (isOriginal)
        RoverManager.Instance.RegisterRover(gameObject);
      Conveyor.OnRoverStepOnConveyor += RoverOnConveyor;
      Conveyor.OnRoverStepOffConveyor += RoverOffConveyor;
    }

    private void Update() {
      if (Conveyored) return;
      if (!RoverManager.Instance.isControlled) return;
      var movement = Input.GetAxisRaw("Horizontal");

      if (movement < 0) {
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
      }
      else if (movement > 0) {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
      }

      dirX = movement * moveSpeed;
    }

    private void RoverOnConveyor(int incomingID, float newSpeed) {
      if (incomingID == RoverId) {
        Conveyored = true;
        dirX = newSpeed;
      }
    }

    private void RoverOffConveyor(int incomingID) {
      if (incomingID == RoverId) {
        Conveyored = false;
        dirX = moveSpeed;
      }
    }

    private void FixedUpdate() {
#if UNITY_6000
      rb.linearVelocity = new Vector2(dirX, 0f);
#else
      rb.velocity = new Vector2(dirX, 0f);
#endif
    }

    public void KillRover() {
      if (!GameObject.Find("Rover clone"))
        roverController.RespawnRover();
      Destroy(gameObject);
    }

    private void OnDestroy() {
      RoverManager.Instance.DeregisterRover(RoverId);
    }
  }
}
