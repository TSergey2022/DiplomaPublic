using System;
using UnityEngine;

namespace Puzzle.Wires_Game {
  public class WiresTile : MonoBehaviour {
    public int tileID;
    public Action<WiresTile> onTileSelected;
    public bool isPressed;

    private void OnMouseDown() {
      isPressed = true;
      InvokeOnSelected();
    }

    private void OnMouseUp() {
      isPressed = false;
      InvokeOnSelected();
    }

    private void InvokeOnSelected() {
      if (onTileSelected != null) onTileSelected.Invoke(this);
    }
  }
}
