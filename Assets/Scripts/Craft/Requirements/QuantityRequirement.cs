using System;
using System.Collections.Generic;
using System.Linq;
using Craft.CraftLib;
using Newtonsoft.Json.Linq;

namespace Craft.Requirements {
  public class QuantityRequirement : IItemRequirement {
    public const string TypeValue = "quantity";
    public const string QuantityTagId = "quantity";

    public IItemRequirement Requirement { get; set; }
    public int RequiredQuantity { get; set; }

    public List<Item> FilterMatchingItems(List<Item> candidates) {
      var matches = Requirement.FilterMatchingItems(candidates);
      var selected = new List<Item>();
      var remainingQuantity = RequiredQuantity;

      foreach (var item in matches) {
        var quantityTag = item.Tags.FirstOrDefault(t => t.Id == QuantityTagId);
        var itemQuantity = quantityTag?.Value is int q ? q : 1;

        if (itemQuantity <= 0) continue;

        var taken = Math.Min(itemQuantity, remainingQuantity);
        var clone = (Item)item.Clone();

        // Adjust quantity tag
        var tagIndex = clone.Tags.FindIndex(t => t.Id == QuantityTagId);
        if (tagIndex >= 0) {
          clone.Tags[tagIndex].Value = taken;
        }
        else {
          clone.Tags.Add(new Tag { Id = QuantityTagId, Value = taken });
        }

        selected.Add(clone);
        remainingQuantity -= taken;
        if (remainingQuantity <= 0)
          break;
      }

      return remainingQuantity <= 0 ? selected : new List<Item>();
    }

    public object Clone() => new QuantityRequirement {
      RequiredQuantity = RequiredQuantity,
      Requirement = (IItemRequirement)Requirement.Clone()
    };

    public JObject ToJObject() => new() {
      [IItemRequirement.TypeMetadataKey] = TypeValue,
      [nameof(RequiredQuantity)] = RequiredQuantity,
      [nameof(Requirement)] = Requirement.ToJObject()
    };

    public static QuantityRequirement FromJObject(JObject json, Func<JObject, IItemRequirement> resolver) {
      return new QuantityRequirement {
        RequiredQuantity = json[nameof(RequiredQuantity)]!.ToObject<int>(),
        Requirement = resolver((JObject)json[nameof(Requirement)]!)
      };
    }
  }
}
