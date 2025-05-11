using UnityEngine;
using UnityEngine.Events;

namespace Puzzle {
  public class Tile : MonoBehaviour {
    public static int UNPLAYABLE_INDEX = 0;
    public static Color COLOR_HIGHTLIGHT = new(1, 1, 0, 0.05f);
    public static string NAME_CONNECTION = "Connection";
    public static string NAME_BACK = "Back";
    public static string NAME_MAK = "Mark";

    public int cid;
    [HideInInspector] public UnityEvent<Tile> onSelected;

    public bool isSelected {
      get { return _isSelected; }
      private set { _isSelected = value; }
    }

    public bool isHighlighted {
      get { return _isHighlighted; }
      private set { _isHighlighted = value; }
    }

    public bool isSolved {
      get { return _isSolved; }
      set { _isSolved = value; }
    }

    public bool isPlayble {
      get { return _isPlayble; }
      private set { _isPlayble = value; }
    }

    private SpriteRenderer BackComponentRenderer {
      get { return transform.Find(NAME_BACK).gameObject.GetComponent<SpriteRenderer>(); }
    }

    public Color ConnectionColor {
      get { return ConnectionComponentRenderer.color; }
      private set { }
    }

    public SpriteRenderer ConnectionComponentRenderer {
      get {
        return transform.Find(NAME_CONNECTION)
          .gameObject.transform.Find("Pipe")
          .gameObject.GetComponent<SpriteRenderer>();
      }
      private set { }
    }

    private SpriteRenderer MarkComponentRenderer {
      get { return transform.Find(NAME_MAK).gameObject.GetComponent<SpriteRenderer>(); }
    }

    [SerializeField] private bool _isSolved;
    private bool _isHighlighted;
    [SerializeField] private bool _isPlayble;
    private bool _isSelected;
    public bool _isPartOfConnection;

    private Color _originalColor;

    private void Start() {
      _isPlayble = cid > UNPLAYABLE_INDEX;
      _originalColor = BackComponentRenderer.color;
      if (_isPlayble)
        SetConnectionColor(MarkComponentRenderer.color);
      else
        Destroy(MarkComponentRenderer.gameObject);
    }

    public void ResetConnection() {
      var connection = transform.Find(NAME_CONNECTION).gameObject;
      connection.SetActive(false);
      connection.transform.eulerAngles = Vector3.zero;
      Debug.Log("Tile -> Reset(" + _isSolved + "): " + cid);
      _isSolved = false;
      _isPartOfConnection = false; // Ensure tile is marked as unconnected
    }

    public void HightlightReset() {
      _isHighlighted = false;
      BackComponentRenderer.color = _originalColor;
    }

    public void Highlight() {
      _isHighlighted = true;
      BackComponentRenderer.color = COLOR_HIGHTLIGHT;
    }

    public void SetConnectionColor(Color color) {
      ConnectionComponentRenderer.color = color;
    }

    public void ConnectionToSide(bool top, bool rigth, bool bottom, bool left) {
      Debug.Log("Tile -> ConnectionToSide: " + top + "|" + rigth + "|" + bottom + "|" + left);
      transform.Find(NAME_CONNECTION).gameObject.SetActive(true);
      var angle = rigth ? -90 : bottom ? -180 : left ? -270 : 0;
      transform.Find(NAME_CONNECTION).gameObject.transform.Rotate(new Vector3(0, 0, angle));
    }

    private void OnMouseUp() {
      if (_isPlayble || _isPartOfConnection) {
        _isSelected = false;
        InvokeOnSelected();
      }
    }

    private void OnMouseDown() {
      if (_isPlayble) {
        _isSelected = true;
        InvokeOnSelected();
      }
    }

    private void InvokeOnSelected() {
      Debug.Log("Tile -> InvokeOnSelected(" + cid + ")");
      if (onSelected != null) onSelected.Invoke(GetComponent<Tile>());
    }
  }
}
