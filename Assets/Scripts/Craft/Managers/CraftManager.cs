using System.Collections.Generic;
using System.Linq;
using Craft.CraftLib;
using Craft.Data;
using Craft.Processors;
using Craft.Requirements;
using Newtonsoft.Json.Linq;

namespace Craft.Managers {
  public class CraftManager {
    public const string ItemDBKey = nameof(itemDB);
    public static CraftManager Instance { get; private set; }

    private ItemDB itemDB;
    private Dictionary<string, List<Receipt>> receiptDB;

    public static void Instantiate(JObject itemDBJson, JObject receiptDBJson) {
      if (Instance == null) {
        Instance = new CraftManager {
          itemDB = ItemDB.FromJObject(itemDBJson),
          receiptDB = receiptDBJson
            .Properties()
            .ToDictionary(
              prop => prop.Name,
              prop => ((JArray)prop.Value)
                .Select(j => Receipt.FromJObject((JObject)j, RequirementResolver.Resolve, ProcessorResolver.Resolve))
                .ToList()
            )
        };
        Instance.itemDB.CacheSprites();
      }
    }

    public List<Receipt> GetReceipts(string containerId) {
      return receiptDB[containerId];
    }

    public ItemDB_Entry GetItem(string id) {
      return itemDB.GetNew(id);
    }

    public static bool TryCraft(Receipt receipt, ref List<Item> items) {
      var remaining = new List<Item>(items.Select(i => (Item)i.Clone()));

      var inputItems = new List<Item>();

      foreach (var slot in receipt.Inputs) {
        var matches = slot.FilterMatchingItems(remaining);

        if (!matches.Any()) {
          return false;
        }

        // For each selected item, reduce quantity from the source pool
        foreach (var used in matches) {
          var usedQty = GetQuantity(used);
          var matchInRemaining = remaining.FirstOrDefault(i => AreStructurallyEqual(i, used));
          if (matchInRemaining == null) continue;

          var availableQty = GetQuantity(matchInRemaining);

          if (usedQty > availableQty) return false;

          // Decrease or remove from remaining
          if (usedQty == availableQty) {
            remaining.Remove(matchInRemaining);
          }
          else {
            var quantityTag = matchInRemaining.Tags.First(t => t.Id == QuantityRequirement.QuantityTagId);
            quantityTag.Value = availableQty - usedQty;
          }

          inputItems.Add(used);
        }
      }

      var outputs = receipt.Output.GenerateOutput(inputItems);
      items = remaining.Concat(outputs).ToList();
      return true;
    }

    private static int GetQuantity(Item item) {
      var tag = item.Tags.FirstOrDefault(t => t.Id == QuantityRequirement.QuantityTagId);
      return tag?.Value is int q ? q : 1;
    }

    private static bool AreStructurallyEqual(Item a, Item b) {
      // Assuming structural match: same ID + same tags except quantity
      if (a.Id != b.Id) return false;
      var tagsA = a.Tags.Where(t => t.Id != QuantityRequirement.QuantityTagId).OrderBy(t => t.Id).ToList();
      var tagsB = b.Tags.Where(t => t.Id != QuantityRequirement.QuantityTagId).OrderBy(t => t.Id).ToList();
      if (tagsA.Count != tagsB.Count) return false;

      for (var i = 0; i < tagsA.Count; i++) {
        if (tagsA[i].Id != tagsB[i].Id || !Equals(tagsA[i].Value, tagsB[i].Value)) return false;
      }

      return true;
    }
  }
}
