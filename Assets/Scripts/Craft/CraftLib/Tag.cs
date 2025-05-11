using System;
using Newtonsoft.Json.Linq;

namespace Craft.CraftLib {
  public class Tag : ICloneable {
    private const string TypeMetadataKey = "_type";
    
    public string Id { get; set; }
    public object Value { get; set; }

    public JObject ToJObject() {
      var json = new JObject {
        [nameof(Id)] = Id
      };

      if (Value != null) {
        var type = Value.GetType().FullName;
        json[nameof(Value)] = JToken.FromObject(Value);
        json[TypeMetadataKey] = type;
      } else {
        json[TypeMetadataKey] = typeof(void).FullName;
      }

      return json;
    }

    public static Tag FromJObject(JObject json) {
      var tag = new Tag {
        Id = (string)json[nameof(Id)]
      };

      var type = (string)json[TypeMetadataKey];
      if (type == typeof(bool).FullName) {
        tag.Value = json[nameof(Value)].ToObject<bool>();
      } else if (type == typeof(int).FullName) {
        tag.Value = json[nameof(Value)].ToObject<int>();
      } else if (type == typeof(float).FullName) {
        tag.Value = json[nameof(Value)].ToObject<float>();
      } else if (type == typeof(string).FullName) {
        tag.Value = json[nameof(Value)].ToObject<string>();
      } else {
        tag.Value = null;
      }

      return tag;
    }

    public object Clone() {
      return new Tag {
        Id = Id,
        Value = Value is ICloneable cloneable ? cloneable.Clone() : Value
      };
    }
  }
}
