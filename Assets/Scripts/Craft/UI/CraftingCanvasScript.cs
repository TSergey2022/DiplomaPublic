using System.Collections.Generic;
using System.Linq;
using Craft.CraftLib;
using Craft.Managers;
using Craft.World;
using UnityEngine;
using UnityEngine.UI;

namespace Craft.UI {
  public class CraftingCanvasScript : MonoBehaviour {
    private readonly List<Item> _craftingInventory = new();
    private IReadOnlyList<Receipt> Receipts { get; set; }
    public GameObject itemsScrollViewContent;
    public GameObject craftScrollViewContent;
    public GameObject recipesScrollViewContent;
    public Button closeButton;
    public Button craftButton;
    public GameObject itemPrefab;
    public GameObject inputPrefab;
    public GameObject recipePrefab;

    private Receipt _currentReceipt;

    public void CloseCraftingCanvas() {
      gameObject.SetActive(false);
    }

    public void OpenCraftingCanvas() {
      gameObject.SetActive(true);
    }

    private void Start() {
      UpdateView();
    }

    private void UpdateView_Inventory() {
      for (var i = itemsScrollViewContent.transform.childCount - 1; i >= 0; i--) {
        Destroy(itemsScrollViewContent.transform.GetChild(i).gameObject);
      }

      if (SaveManager.Instance.SaveData.Inventory.Count == 0) return;

      foreach (var item in SaveManager.Instance.SaveData.Inventory) {
        var itemGo = Instantiate(itemPrefab, itemsScrollViewContent.transform);
        itemGo.GetComponent<CraftItemElement>().SetCraftItem(item, () => {
          if (_currentReceipt == null) return;
          var tempItem = item.Clone();
          // tempItem.quantity++;
          // GodScript.God.GetInventory()[item].quantity--;
          _craftingInventory.Add((Item)tempItem);
          UpdateView_Inventory();
          UpdateView_Inputs();
        });
      }
    }

    private void UpdateView_Inputs() {
      for (var i = craftScrollViewContent.transform.childCount - 1; i >= 0; i--) {
        Destroy(craftScrollViewContent.transform.GetChild(i).gameObject);
      }

      craftButton.interactable = _currentReceipt != null;
      if (_currentReceipt == null) {
        craftScrollViewContent.SetActive(false);
      }
      else {
        craftScrollViewContent.SetActive(true);
        foreach (var item in _craftingInventory) {
          var itemGo = Instantiate(itemPrefab, craftScrollViewContent.transform);
          itemGo.GetComponent<CraftItemElement>().SetCraftItem(item, () => {
            // item.Quantity -= 1;
            // if (item.Quantity == 0) craftingInventory.RemoveItem(item);
            SaveManager.Instance.SaveData.Inventory.Add(CraftManager.Instance.GetItem(item.Id).Etalon);
            UpdateView_Inventory();
            UpdateView_Inputs();
          });
        }
      }
    }

    private void UpdateView_Recipes() {
      for (var i = recipesScrollViewContent.transform.childCount - 1; i >= 0; i--) {
        Destroy(recipesScrollViewContent.transform.GetChild(i).gameObject);
      }

      foreach (var receipt in Receipts) {
        var recipeGo = Instantiate(recipePrefab, recipesScrollViewContent.transform);
        recipeGo.GetComponent<CraftReceiptElement>().SetCraftReceipt(receipt, () => SetActiveRecipe(receipt));
      }
    }

    private void UpdateView() {
      UpdateView_Inventory();
      UpdateView_Inputs();
      UpdateView_Recipes();
    }

    public void SetReceipts(List<Receipt> receipts) {
      Receipts = receipts;
      SetActiveRecipe(null);
      UpdateView();
    }

    public void SetReceipts(ReceiptContainerScript receiptContainer) {
      Receipts = receiptContainer.GetReceipts();
      SetActiveRecipe(null);
      UpdateView();
    }

    public void SetActiveRecipe(Receipt receipt) {
      if (receipt == _currentReceipt) return;
      _craftingInventory.ToList().ForEach(i => { SaveManager.Instance.SaveData.Inventory.Add(i); });
      _craftingInventory.Clear();
      _currentReceipt = receipt;
    }

    public void Craft() {
      // if (_currentReceipt == null) return;
      // var tempInventory = new List<CraftLib.Item>();
      // var success = true;
      // foreach (var input in _currentReceipt.Inputs.Values) {
      //   var items = craftingInventory;
      //   if (items.Count == 0) {
      //     if (input.CheckRequirements(new CraftSpace { Item = null })) continue;
      //   }
      //   var quantityNeed = input.Quantity;
      //   foreach (var item in items) {
      //     if (!input.CheckRequirements(new CraftSpace { Item = item })) continue;
      //     var minQuantity = Mathf.Min(quantityNeed, item.Quantity);
      //     tempInventory.Add(new CraftItem(item, minQuantity));
      //     quantityNeed -= minQuantity;
      //     if (quantityNeed == 0) break;
      //   }
      //   tempInventory.ForEach((i) => craftingInventory.RemoveItem(i.Id, i.Quantity));
      //   if (quantityNeed != 0) {
      //     success = false;
      //     break;
      //   }
      // }
      //
      // if (success) {
      //   tempInventory.Clear();
      //   _currentReceipt.Outputs.ToList().ForEach(item => {
      //     GodScript.God.GetInventory().Add((CraftLib.Item)item.Clone());
      //   });
      // }
      // else {
      //   tempInventory.Reverse();
      //   tempInventory.ForEach((i) => craftingInventory.AddItem(i, 0));
      // }
      // UpdateView_Inventory();
      // UpdateView_Inputs();
    }
  }
}
