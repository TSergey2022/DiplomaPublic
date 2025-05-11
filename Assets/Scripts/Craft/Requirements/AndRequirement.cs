using System;
using System.Collections.Generic;
using System.Linq;
using Craft.CraftLib;
using Newtonsoft.Json.Linq;

namespace Craft.Requirements {
  public class AndRequirement : IItemRequirement {
    public const string TypeValue = "and";

    public List<IItemRequirement> Requirements { get; set; } = new();

    public List<Item> FilterMatchingItems(List<Item> candidates) {
      var matches = new List<Item>();
      foreach (var item in candidates) {
        var allSatisfied = true;
        foreach (var req in Requirements) {
          if (!req.FilterMatchingItems(new List<Item> { item }).Any()) {
            allSatisfied = false;
            break;
          }
        }
        if (allSatisfied) matches.Add(item);
      }
      return matches;
    }


    public object Clone() {
      return new AndRequirement {
        Requirements = Requirements.ConvertAll(r => (IItemRequirement)r.Clone())
      };
    }

    public JObject ToJObject() {
      return new JObject {
        [IItemRequirement.TypeMetadataKey] = TypeValue,
        [nameof(Requirements)] = new JArray(Requirements.ConvertAll(r => r.ToJObject()))
      };
    }

    public static AndRequirement FromJObject(JObject json, Func<JObject, IItemRequirement> requirementResolver) {
      var reqs = new List<IItemRequirement>();
      foreach (var token in (JArray)json[nameof(Requirements)]) {
        reqs.Add(requirementResolver(token.Value<JObject>()));
      }

      return new AndRequirement { Requirements = reqs };
    }
  }
}
