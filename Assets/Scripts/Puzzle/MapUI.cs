using UnityEngine;

namespace Puzzle {
  public class MapUI : MonoBehaviour {
    private ShipZone[,] Rooms;

    [SerializeField] private GameObject Map;
    private (int, int) markedRoom = (-1, -1);

    private int _dimensionX = 3;
    private int _dimensionY = 3;

    public static MapUI Instance { get; private set; }

    private void Awake() {
      if (Instance != null && Instance != this) {
        Destroy(gameObject);
        return;
      }

      Instance = this;
      // DontDestroyOnLoad(gameObject); // DontDestroyOnLoad only works for root GameObjects or components on root GameObjects.

      InitializeMap();
      GameController.instance.OnMapOpen += Switch;
      MarkOnMap(0, 0);

      Switch(false);
    }

    public void MarkOnMap(int x, int y) {
      if (markedRoom != (-1, -1))
        Rooms[markedRoom.Item1, markedRoom.Item2].RemoveMark();
      markedRoom = (x, y);
      Rooms[markedRoom.Item1, markedRoom.Item2].AddMark();
    }

    private void OnEnable() {
      foreach (var zone in Rooms) {
        if (zone.relationshipWithFactionRequired <= FactionManager.Instance.GetFactionRelationship(zone.HostFaction))
          zone.Unlock();
      }
    }

    private void InitializeMap() {
      Rooms = new ShipZone[_dimensionX, _dimensionY];
      for (var y = 0; y < _dimensionX; y++) {
        if (transform.GetChild(y).CompareTag("Row")) {
          var row = transform.GetChild(y).transform;
          for (var x = 0; x < _dimensionY; x++) {
            if (row.GetChild(x).CompareTag("Room")) {
              var room = row.GetChild(x).gameObject;
              Rooms[x, y] = room.GetComponent<ShipZone>();
              Rooms[x, y].SetZoneCoordinates(x, y);
            }
          }
        }
      }
    }


    private void Switch(bool val) {
      Map.SetActive(val);
    }

    private void Update() { }
  }
}
