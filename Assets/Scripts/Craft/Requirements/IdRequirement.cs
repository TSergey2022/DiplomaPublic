using System.Collections.Generic;
using Craft.CraftLib;
using Newtonsoft.Json.Linq;

namespace Craft.Requirements {
  public class IdRequirement : IItemRequirement {
    public const string TypeValue = "id";

    public string RequiredId { get; set; }

    public List<Item> FilterMatchingItems(List<Item> candidates) =>
      candidates.FindAll(i => i?.Id == RequiredId);

    public object Clone() => new IdRequirement { RequiredId = RequiredId };

    public JObject ToJObject() {
      return new JObject {
        [IItemRequirement.TypeMetadataKey] = TypeValue,
        [nameof(RequiredId)] = RequiredId
      };
    }

    public static IdRequirement FromJObject(JObject json) {
      return new IdRequirement { RequiredId = json[nameof(RequiredId)]!.ToString() };
    }
  }
}
