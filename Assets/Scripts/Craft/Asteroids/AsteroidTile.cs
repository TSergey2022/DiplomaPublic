using System;
using Craft.CraftLib;
using Newtonsoft.Json.Linq;

namespace Craft.Asteroids {
  public class AsteroidTile : ICloneable {
    public string Id { get; set; }
    public float Durability { get; set; }
    public Item Drop { get; set; }

    public JObject ToJObject() {
      var json = new JObject {
        [nameof(Id)] = Id,
        [nameof(Durability)] = Durability
      };
      if (Drop != null) {
        json[nameof(Drop)] = Drop.ToJObject();
      }
      return json;
    }

    public object Clone() {
      return new AsteroidTile {
        Id = Id,
        Durability = Durability,
        Drop = Drop?.Clone() as Item
      };
    }

    public static AsteroidTile FromJObject(JObject json) {
      var asteroidTile = new AsteroidTile {
        Id = json.Value<string>(nameof(Id)),
        Durability = json.Value<float>(nameof(Durability))
      };
      if (json.TryGetValue(nameof(Drop), out var value) && value.Type == JTokenType.Object) {
        asteroidTile.Drop = Item.FromJObject((JObject)value);
      }
      return asteroidTile;
    }
  }
}
