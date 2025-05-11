using Craft.Managers;
using UnityEngine;

namespace Craft.UI {
  public class InventoryScript : MonoBehaviour {
    [SerializeField] private GameObject itemPrefab;

    [SerializeField] private GameObject content;

    public void OnEnable() {
      content.transform.DetachChildren();
      var items = SaveManager.Instance.SaveData.Inventory;
      for (var i = 0; i < items.Count; i++) {
        var item = items[i];
        var itemGO = Instantiate(itemPrefab, content.transform, true);
        var element = itemGO.GetComponent<CraftItemElement>();
        element.SetCraftItem(item, null);
      }
    }
  }
}
