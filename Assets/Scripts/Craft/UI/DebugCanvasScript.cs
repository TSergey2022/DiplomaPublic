using Craft.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Craft.UI {
  [RequireComponent(typeof(Canvas))]
  public class DebugCanvasScript : MonoBehaviour {
    [SerializeField] private InputField inputField;
    private Canvas _canvas;

    private void Awake() {
      if (inputField == null) Debug.LogError("Input field is null");
      _canvas = GetComponent<Canvas>();
    }

    private void Update() {
      if (Input.GetKeyDown(KeyCode.BackQuote)) {
        inputField.text = "";
        _canvas.enabled = !_canvas.enabled;
      }

      if (Input.GetKey(KeyCode.LeftControl)) {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
          FadeManager.Instance.FadeOut();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
          FadeManager.Instance.FadeIn();
        }
      }
    }

    public void Apply() {
      var str = inputField.text;
      try {
        var strs = str.Split(' ');
        var cmd = strs[0];
        if (cmd == "add_item") {
          var itemId = strs[1];
          var itemCount = int.Parse(strs[2]);
          var item = CraftManager.Instance.GetItem(itemId).Etalon;
          SaveManager.Instance.SaveData.Inventory.Add(item);
          Debug.Log($"({itemCount}) x [{itemId}] added to Inventory");
        }

        if (cmd == "remove_item") {
          var itemId = strs[1];
          var itemCount = int.Parse(strs[2]);
          var item = SaveManager.Instance.SaveData.Inventory.Find((i) => i.Id == itemId);
          SaveManager.Instance.SaveData.Inventory.Remove(item);
          Debug.Log($"({itemCount}) x [{itemId}] removed from Inventory");
        }
      }
      catch {
        Debug.LogError("Something went wrong");
      }
      finally {
        inputField.text = "";
      }
    }
  }
}
