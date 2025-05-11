using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Craft.CraftLib {
  public class Item : ICloneable {
    public string Id { get; set; }
    public List<Tag> Tags { get; set; } = new(); // Инициализация по умолчанию

    public JObject ToJObject() => new() {
      [nameof(Id)] = Id,
      [nameof(Tags)] = new JArray(Tags.Select(t => t.ToJObject()))
    };

    public object Clone() => new Item {
      Id = Id,
      Tags = Tags.Select(t => (Tag)t.Clone()).ToList()
    };

    public static Item FromJObject(JObject json) => new() {
      Id = (string)json[nameof(Id)],
      Tags = json[nameof(Tags)]
        .OfType<JObject>()
        .Select(Tag.FromJObject)
        .ToList()
    };
  }
}
