using System;
using UnityEngine;

namespace Puzzle {
  public enum ButtonColorType {
    Green,
    Magenta,
    Multiple
  }

  public class ActivatableObject : MonoBehaviour {
    public ButtonColorType objectColor;
  }


  public class ColorBasedActivatorObject : MonoBehaviour {
    public static Action<ButtonColorType> OnActivatorButtonPressed;
    public ButtonColorType buttonColor;

    private void OnTriggerEnter2D(Collider2D collision) {
      if (collision.CompareTag("Player") || collision.CompareTag("Rover")) {
        OnActivatorButtonPressed?.Invoke(buttonColor);
        Debug.Log("Object activated!");
      }
    }
  }
}
