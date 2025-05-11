using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Craft.CraftLib {
  public class InputSlot : ICloneable {
    public string Name { get; set; }
    public IItemRequirement Requirement { get; set; }

    public List<Item> FilterMatchingItems(List<Item> items) => Requirement.FilterMatchingItems(items);

    public object Clone() => new InputSlot {
      Name = Name,
      Requirement = (IItemRequirement)Requirement.Clone()
    };

    public JObject ToJObject() => new() {
      [nameof(Name)] = Name,
      [nameof(Requirement)] = Requirement.ToJObject()
    };

    public static InputSlot FromJObject(JObject json, Func<JObject, IItemRequirement> requirementResolver) {
      return new InputSlot {
        Name = json[nameof(Name)]!.ToString(),
        Requirement = requirementResolver((JObject)json[nameof(Requirement)]!)
      };
    }
  }
}
