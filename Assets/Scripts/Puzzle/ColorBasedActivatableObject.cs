using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
  public class ColorBasedActivatableObject : MonoBehaviour {
    public enum ObjectActivator {
      Field,
      Buttons
    }

    [SerializeField] public ObjectActivator objectActivator;
    [SerializeField] public ButtonColorType ActivatableObjectColor;
    [SerializeField] public List<ActivatableLight> lights = new();
  }
}
