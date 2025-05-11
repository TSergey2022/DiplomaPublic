using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Craft.CraftLib {
  public class Receipt : ICloneable {
    public string Id { get; set; }
    public List<InputSlot> Inputs { get; set; } = new();
    public OutputSlot Output { get; set; }

    public object Clone() {
      return new Receipt {
        Id = Id,
        Inputs = Inputs.ConvertAll(i => (InputSlot)i.Clone()),
        Output = (OutputSlot)Output.Clone()
      };
    }

    public JObject ToJObject() {
      return new JObject {
        [nameof(Id)] = Id,
        [nameof(Inputs)] = new JArray(Inputs.ConvertAll(i => i.ToJObject())),
        [nameof(Output)] = Output.ToJObject()
      };
    }

    public static Receipt FromJObject(JObject json, Func<JObject, IItemRequirement> requirementResolver, Func<JObject, IItemProcessor> processorResolver) {
      return new Receipt {
        Id = json[nameof(Id)]!.ToString(),
        Inputs = ((JArray)json[nameof(Inputs)]!).ToObject<List<JObject>>()!.ConvertAll(i => InputSlot.FromJObject(i, requirementResolver)),
        Output = OutputSlot.FromJObject((JObject)json[nameof(Output)]!, processorResolver)
      };
    }
  }
}
