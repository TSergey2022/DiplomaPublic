using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Puzzle {
  public class ShipZone : MonoBehaviour, IPointerClickHandler {
    [SerializeField] public FactionType HostFaction;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject Mark;
    [SerializeField] public int relationshipWithFactionRequired;
    [SerializeField] private int zoneHubSceneIndex;
    private int _x;
    private int _y;

    private bool locked = true;

    public void SetZoneCoordinates(int x, int y) {
      _x = x;
      _y = y;
    }

    public void Unlock() {
      lockIcon.SetActive(false);
      locked = false;
    }

    public void Lock() {
      lockIcon.SetActive(true);
      locked = true;
    }

    public void AddMark() {
      Mark.SetActive(true);
    }

    public void RemoveMark() {
      Mark.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) {
      if (!locked)
        MoveToZone();
      else
        Debug.Log("They dont want you here mate");
    }

    private void MoveToZone() {
      MapUI.Instance.MarkOnMap(_x, _y);
      SceneManager.LoadScene(zoneHubSceneIndex);
    }
  }
}
