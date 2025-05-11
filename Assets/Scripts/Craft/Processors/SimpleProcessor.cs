using System;
using System.Collections.Generic;
using System.Linq;
using Craft.CraftLib;
using Newtonsoft.Json.Linq;

namespace Craft.Processors {
  public class SimpleProcessor : IItemProcessor {
    private const string TypeValue = "simple";

    public List<Item> OutputItems { get; set; } = new();

    public List<Item> Process(List<Item> inputItems) {
      // Consumes inputItems completely, returns cloned output
      return OutputItems.Select(item => (Item)item.Clone()).ToList();
    }

    public object Clone() => new SimpleProcessor {
      OutputItems = OutputItems.Select(i => (Item)i.Clone()).ToList()
    };

    public JObject ToJObject() => new() {
      [IItemProcessor.TypeMetadataKey] = TypeValue,
      [nameof(OutputItems)] = new JArray(OutputItems.Select(i => i.ToJObject()))
    };

    public static SimpleProcessor FromJObject(JObject json) {
      return new SimpleProcessor {
        OutputItems = json[nameof(OutputItems)]
          .OfType<JObject>()
          .Select(Item.FromJObject)
          .ToList()
      };
    }
  }
}
