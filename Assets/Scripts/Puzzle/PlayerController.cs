using Puzzle.Portal_Game;
using UnityEngine;

namespace Puzzle {
  public class PlayerController : MonoBehaviour {
    public static PlayerController Instance { get; private set; }
    private IControllable currentControlledEntity;
    private IControllable characterControllable;
    private IControllable roverControllable;

    [SerializeField] private GameObject character;
    [SerializeField] private GameObject rover;

    private void Awake() {
      Instance = this;
      characterControllable = character.GetComponent<MoveCharacter>();
      roverControllable = rover.GetComponent<RoverManager>();
      currentControlledEntity = characterControllable;
    }

    public void SwitchControl(IControllable newEntity) {
      if (currentControlledEntity != null)
        currentControlledEntity.SetControl(false);

      currentControlledEntity = newEntity;
      currentControlledEntity.SetControl(true);
    }

    private void Update() {
      if (Input.GetKeyDown(KeyCode.Tab)) {
        SwitchControl(currentControlledEntity == characterControllable ? roverControllable : characterControllable);
      }
    }
  }

  public interface IControllable {
    void SetControl(bool isActive);
  }
}
