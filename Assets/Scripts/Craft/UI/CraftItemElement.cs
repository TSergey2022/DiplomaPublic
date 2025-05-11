using System;
using Craft.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Craft.UI {
  public class CraftItemElement : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private Text text;
    [SerializeField] private Button button;

    public void SetCraftItem(CraftLib.Item item, Action onClickCallback) {
      var entry = CraftManager.Instance.GetItem(item.Id);
      image.sprite = entry.Icon;
      // text.text = $"{item.GetQuantity()} x {entry.Name}";
      button.onClick.RemoveAllListeners();
      if (onClickCallback != null) {
        button.onClick.AddListener(() => onClickCallback());
      }
    }
  }
}
