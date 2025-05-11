using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Craft.CraftLib {
  public class OutputSlot : ICloneable {
    public string Name { get; set; }
    public IItemProcessor Processor { get; set; }

    public List<Item> GenerateOutput(List<Item> inputItems) => Processor.Process(inputItems);

    public object Clone() => new OutputSlot {
      Name = Name,
      Processor = (IItemProcessor)Processor.Clone()
    };

    public JObject ToJObject() => new() {
      [nameof(Name)] = Name,
      [nameof(Processor)] = Processor.ToJObject()
    };

    public static OutputSlot FromJObject(JObject json, Func<JObject, IItemProcessor> processorResolver) {
      return new OutputSlot {
        Name = json[nameof(Name)]!.ToString(),
        Processor = processorResolver((JObject)json[nameof(Processor)]!)
      };
    }
  }
}
