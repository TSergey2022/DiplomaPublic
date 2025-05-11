using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Craft.CraftLib {
  public interface IItemRequirement : ICloneable {
    public const string TypeMetadataKey = "_type";
    List<Item> FilterMatchingItems(List<Item> candidates);
    JObject ToJObject();
  }
}
