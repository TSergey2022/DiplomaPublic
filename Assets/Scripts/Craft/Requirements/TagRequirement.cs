using System.Collections.Generic;
using Craft.CraftLib;
using Newtonsoft.Json.Linq;

namespace Craft.Requirements {
  public class TagRequirement : IItemRequirement {
    public const string TypeValue = "tag";

    public string TagId { get; set; }

    public List<Item> FilterMatchingItems(List<Item> candidates) =>
      candidates.FindAll(i => i?.Tags?.Exists(t => t.Id == TagId) == true);

    public object Clone() => new TagRequirement { TagId = TagId };

    public JObject ToJObject() {
      return new JObject {
        [IItemRequirement.TypeMetadataKey] = TypeValue,
        [nameof(TagId)] = TagId
      };
    }

    public static TagRequirement FromJObject(JObject json) {
      return new TagRequirement { TagId = json[nameof(TagId)]!.ToString() };
    }
  }
}
