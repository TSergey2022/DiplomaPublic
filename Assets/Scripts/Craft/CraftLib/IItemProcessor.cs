using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Craft.CraftLib {
  public interface IItemProcessor : ICloneable {
    public const string TypeMetadataKey = "_type";
    List<Item> Process(List<Item> inputItems);
    JObject ToJObject();
  }
}
