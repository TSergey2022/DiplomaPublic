using System;
using UnityEngine;

namespace Puzzle {
  public class ActivatableLight : MonoBehaviour {
    public ButtonColorType lightColor;
    private SpriteRenderer spriteRenderer;
    public bool lightActivated;

    public static Action OnLightActivated;

    private void Start() {
      spriteRenderer = GetComponent<SpriteRenderer>();
      ColorBasedActivatorObject.OnActivatorButtonPressed += ActivateLight;
    }

    private void ActivateLight(ButtonColorType buttonColor) {
      if (buttonColor == lightColor) {
        var color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
        lightActivated = true;
        OnLightActivated?.Invoke();
      }
    }
  }
}
