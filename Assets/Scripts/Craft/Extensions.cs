using UnityEngine;

namespace Craft {
  public static class Extensions {
    public static void DestroyAllChildren(this Transform transform) {
      if (transform.childCount == 0) return;
      for (var i = transform.childCount - 1; i >= 0; i--) {
        Object.Destroy(transform.GetChild(i).gameObject);
      }
    }
  }
}
