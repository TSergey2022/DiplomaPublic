using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Craft.Asteroids {
  public class Asteroid : ICloneable {
    public Dictionary<Vector3Int, AsteroidTile> Tiles { get; set; } = new();

    public JObject ToJObject() {
      var json = new JObject {
        [nameof(Tiles)] = JObject.FromObject(
          Tiles.ToDictionary(
            k => VectorToKey(k.Key),
            v => v.Value.ToJObject()
          )
        )
      };
      return json;
    }

    public bool IsEmpty() {
      return Tiles.Count == 0;
    }

    public object Clone() {
      var asteroid = new Asteroid {
        Tiles = Tiles.ToDictionary(
          k => k.Key,
          v => v.Value.Clone() as AsteroidTile
        )
      };
      return asteroid;
    }

    public static Asteroid FromJObject(JObject json) {
      var asteroid = new Asteroid();
      var tiles = new Dictionary<Vector3Int, AsteroidTile>();
      foreach (var (key, value) in (JObject)json[nameof(Tiles)]) {
        tiles[KeyToVector(key)] = AsteroidTile.FromJObject((JObject)value);
      }
      asteroid.Tiles = tiles;
      return asteroid;
    }

    private static string VectorToKey(Vector3Int v) => $"{v.x},{v.y},{v.z}";

    private static Vector3Int KeyToVector(string s) {
      var parts = s.Split(',').Select(int.Parse).ToArray();
      if (parts.Length != 3)
        throw new FormatException($"Invalid vector key format: '{s}'");
      return new Vector3Int(parts[0], parts[1], parts[2]);
    }
  }
}
