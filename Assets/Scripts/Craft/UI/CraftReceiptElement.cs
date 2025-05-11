using System;
using Craft.CraftLib;
using Craft.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Craft.UI {
  public class CraftReceiptElement : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private Text text;
    [SerializeField] private Button button;

    public void SetCraftReceipt(Receipt receipt, Action onClickCallback) {
      // var itemId = receipt.Outputs.First().Id;
      var itemId = "";
      var entry = CraftManager.Instance.GetItem(itemId);
      image.sprite = entry.Icon;
      text.text = entry.Name;
      // text.text = $"{entry.Name}\n"
      //             + string.Join("\n", receipt.Inputs.Select(o => o.ToString()).ToArray());
      button.onClick.RemoveAllListeners();
      button.onClick.AddListener(() => onClickCallback());
    }
  }
}
