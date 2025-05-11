using System.Collections.Generic;
using Craft.CraftLib;
using Craft.Managers;
using UnityEngine;

namespace Craft.World {
  public class ReceiptContainerScript : MonoBehaviour {
    [SerializeField] private string containerId;
    private List<Receipt> receipts = new();

    private void Awake() {
      var receipts = CraftManager.Instance.GetReceipts(containerId);
      if (receipts != null) {
        this.receipts = receipts;
      }
    }

    public List<Receipt> GetReceipts() => receipts;

    public void OpenCraftingMenu() {
      GameManager.Instance.OpenCraftingCanvas(this);
    }
  }
}
