using System;
using System.Collections.Generic;
using System.Linq;
using Craft.CraftLib;
using Newtonsoft.Json.Linq;

namespace Craft.Requirements {
  public class OrRequirement : IItemRequirement {
    public const string TypeValue = "or";

    public List<IItemRequirement> Requirements { get; set; } = new();

    public List<Item> FilterMatchingItems(List<Item> candidates) {
      var result = new HashSet<Item>();
      foreach (var req in Requirements) {
        foreach (var item in req.FilterMatchingItems(candidates)) {
          result.Add(item);
        }
      }
      return result.ToList();
    }


    public object Clone() {
      return new OrRequirement {
        Requirements = Requirements.ConvertAll(r => (IItemRequirement)r.Clone())
      };
    }

    public JObject ToJObject() {
      return new JObject {
        [IItemRequirement.TypeMetadataKey] = TypeValue,
        [nameof(Requirements)] = new JArray(Requirements.ConvertAll(r => r.ToJObject()))
      };
    }

    public static OrRequirement FromJObject(JObject json, Func<JObject, IItemRequirement> requirementResolver) {
      var reqs = new List<IItemRequirement>();
      foreach (var token in (JArray)json[nameof(Requirements)]) {
        reqs.Add(requirementResolver(token.Value<JObject>()));
      }

      return new OrRequirement { Requirements = reqs };
    }
  }
}
