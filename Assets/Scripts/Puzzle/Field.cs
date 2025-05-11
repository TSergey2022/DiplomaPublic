using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
  public class Field : MonoBehaviour {
    [SerializeField] private ButtonColorType ActiateObjectColor;
    private Tile[,] _grid;
    private bool _canDrawConnection;

    private List<Tile> _connections = new();
    private Tile _connectionTile;

    private List<int> _solvedConnections = new();

    public static Action<ButtonColorType> OnWiresSolved;
    public static Action<int> OnWireConnected;


    private int _dimensionX;
    private int _dimensionY;
    private int _solved;
    private Dictionary<int, int> _amountToSolve = new();

    private Dictionary<int, List<Tile>> connections = new();

    private void Start() {
      _dimensionX = transform.childCount;
      _dimensionY = transform.GetChild(0).transform.childCount;
      _grid = new Tile[_dimensionY, _dimensionX];

      for (var y = 0; y < _dimensionX; y++) {
        var row = transform.GetChild(y).transform;
        row.gameObject.name = "" + y;
        for (var x = 0; x < _dimensionY; x++) {
          var tile = row.GetChild(x).GetComponent<Tile>();
          tile.gameObject.name = "" + x;
          tile.onSelected.AddListener(onTileSelected);
          _CollectAmountToSolveFromTile(tile);
          _grid[x, y] = tile;
        }
      }

      //SetGameStatus(_solved, _amountToSolve.Count);
      _OutputGrid();
      foreach (var x in _amountToSolve) {
        if (x.Value % 2 != 0)
          Debug.Log("wow ur stupid: " + x.Key);
      }
    }

    private void _CollectAmountToSolveFromTile(Tile tile) {
      if (tile.cid > Tile.UNPLAYABLE_INDEX) {
        if (_amountToSolve.ContainsKey(tile.cid))
          _amountToSolve[tile.cid] += 1;
        else _amountToSolve[tile.cid] = 1;
      }
    }

    private void _OutputGrid() {
      var results = "";
      var dimension = transform.childCount;
      for (var y = 0; y < dimension; y++) {
        results += "{";
        var row = transform.GetChild(y).transform;
        for (var x = 0; x < row.childCount; x++) {
          var tile = _grid[x, y];
          if (x > 0) results += ",";
          results += tile.cid;
        }

        results += "}\n";
      }

      Debug.Log("Main -> Start: _grid: \n" + results);
    }

    private Vector3 _mouseWorldPosition;
    private int _mouseGridX, _mouseGridY;

    private void Update() {
      if (_canDrawConnection) {
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mouseGridX = (int)(Mathf.Floor(_mouseWorldPosition.x) - transform.position.x);
        _mouseGridY = (int)(Mathf.Floor(_mouseWorldPosition.y) - transform.position.y);

        Debug.Log($"Mouse Grid Y: {_dimensionX}, Dimension Y: {_dimensionY}");

        if (_CheckMouseOutsideGrid()) return;

        var hoverTile = _grid[_mouseGridX, _mouseGridY];
        var firstTile = _connections[0];
        /*if (hoverTile._isPartOfConnection)
      {
          Debug.Log("Field -> OnMouseDrag: Erasing intersected connection");
          _EraseConnection(hoverTile);
      }*/
        var isDifferentActiveTile = hoverTile.cid > 0 && hoverTile.cid != firstTile.cid;

        if (hoverTile.isHighlighted || hoverTile.isSolved || isDifferentActiveTile) return;

        var connectionTilePosition = _FindTileCoordinates(_connectionTile);
        var isPositionDifferent = IsDifferentPosition(_mouseGridX, _mouseGridY, connectionTilePosition);

        Debug.Log("Field -> OnMouseDrag(" + isPositionDifferent + "): " + _mouseGridX + "|" + _mouseGridY);

        if (isPositionDifferent) {
          var deltaX = Math.Abs(connectionTilePosition.x - _mouseGridX);
          var deltaY = Math.Abs(connectionTilePosition.y - _mouseGridY);
          var isShiftNotOnNext = deltaX > 1 || deltaY > 1;
          var isShiftDiagonal = (deltaX > 0 && deltaY > 0);
          Debug.Log("Field -> OnMouseDrag: isShiftNotOnNext = " + isShiftNotOnNext + "| isShiftDiagonal = " +
                    isShiftDiagonal);
          if (isShiftNotOnNext || isShiftDiagonal) return;

          hoverTile.Highlight();
          hoverTile.SetConnectionColor(_connectionTile.ConnectionColor);

          _connectionTile.ConnectionToSide(
            _mouseGridY > connectionTilePosition.y,
            _mouseGridX > connectionTilePosition.x,
            _mouseGridY < connectionTilePosition.y,
            _mouseGridX < connectionTilePosition.x
          );


          _connectionTile = hoverTile;
          _connections.Add(_connectionTile);
          _connectionTile._isPartOfConnection = true;

          if (_CheckIfTilesMatch(hoverTile, firstTile)) {
            _connections.ForEach((tile) => tile.isSolved = true);
            _canDrawConnection = false;
            _amountToSolve.Remove(firstTile.cid);
            OnWireConnected?.Invoke(firstTile.cid);

            if (_amountToSolve.Keys.Count == 0) {
              Debug.Log("GAME COMPLETE");
              gameObject.SetActive(false);
              OnWiresSolved?.Invoke(ActiateObjectColor);
            }
          }
        }
      }
    }

    private bool _CheckIfTilesMatch(Tile tile, Tile another) {
      return tile.cid > 0 && another.cid == tile.cid;
    }

    private bool _CheckMouseOutsideGrid() {
      Debug.Log("_mouseGridY >= _dimensionY || _mouseGridY < 0 || _mouseGridX >= _dimensionX || _mouseGridX < 0");
      Debug.Log(
        $"{_mouseGridY} >= {_dimensionY} || {_mouseGridY} < 0 || {_mouseGridX} >= {_dimensionX} || {_mouseGridX} < 0;");
      return _mouseGridY >= _dimensionX || _mouseGridY < 0 || _mouseGridX >= _dimensionY || _mouseGridX < 0;
    }

    private void _EraseConnection(Tile tile) {
      if (_connections.Contains(tile)) {
        foreach (var t in _connections) {
          t.ResetConnection();
          t._isPartOfConnection = false;
          t.HightlightReset();
        }

        _connections.Clear();
      }
    }


    private void onTileSelected(Tile tile) {
      Debug.Log("Field -> onTileSelected(" + tile.isSelected + "): " + _FindTileCoordinates(tile));

      if (tile._isPartOfConnection) {
        Debug.Log("Field -> onTileSelected: Erasing connection");
        _EraseConnection(tile);
        return;
      }

      if (tile.isSelected) {
        _connectionTile = tile;
        _connections = new List<Tile>();
        _connections.Add(_connectionTile);
        _canDrawConnection = true;
        _connectionTile.Highlight();
      }
      else {
        var isFirstTileInConnection = _connectionTile == tile;
        if (isFirstTileInConnection) {
          tile.HightlightReset();
        }
        else if (!_CheckIfTilesMatch(_connectionTile, tile)) {
          _ResetConnections();
        }

        _canDrawConnection = false;
      }

      Debug.Log($"[onTileSelected] _connectionTile set to: {_FindTileCoordinates(_connectionTile)}");
    }


    public void onRestart() {
      Debug.Log("Field -> onRestart");
      var dimension = transform.childCount;
      for (var y = 0; y < dimension; y++) {
        var row = transform.GetChild(y).transform;
        for (var x = 0; x < row.childCount; x++) {
          var tile = _grid[x, y];
          tile.ResetConnection();
          tile.HightlightReset();
          _CollectAmountToSolveFromTile(tile);
        }
      }

      _solved = 0;
    }


    private void _ResetConnections() {
      Debug.Log("Field -> _ResetConnections: Clearing connections");
      foreach (var tile in _connections) {
        tile.ResetConnection();
        tile._isPartOfConnection = false;
        tile.HightlightReset();
      }

      _connections.Clear();
    }

    private Vector2 _FindTileCoordinates(Tile tile) {
      var x = int.Parse(tile.gameObject.name);
      var y = int.Parse(tile.gameObject.transform.parent.gameObject.name);
      return new Vector2(x, y);
    }

    public bool IsDifferentPosition(int gridX, int gridY, Vector2 position) {
      return position.x != gridX || position.y != gridY;
    }

    private class Connection {
      public Tile tile;
      public Vector2 position;

      public Connection(Tile tile, Vector2 position) {
        this.tile = tile;
        this.position = position;
      }

      public bool IsDifferentPosition(int gridX, int gridY) {
        return position.x != gridX || position.y != gridY;
      }
    }
  }
}
