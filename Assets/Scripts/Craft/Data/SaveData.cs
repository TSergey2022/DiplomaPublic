using System;
using System.Collections.Generic;
using System.Linq;
using Craft.Asteroids;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Craft.Data {
  public class SaveData : ICloneable {
    public List<CraftLib.Item> Inventory { get; set; } = new();
    public List<Asteroid> Asteroids { get; set; } = new();
    public HashSet<string> Flags { get; set; } = new();
    public string LocationId { get; set; } = string.Empty;
    public int AsteroidId { get; set; }
    public int StationId { get; set; }
    public Vector3 PlayerPosition { get; set; } = Vector3.zero;

    private static JObject Vector3ToJObject(Vector3 v) => new() {
      [nameof(Vector3.x)] = v.x,
      [nameof(Vector3.y)] = v.y,
      [nameof(Vector3.z)] = v.z
    };

    private static Vector3 JObjectToVector3(JObject obj) => new() {
      x = (float)obj[nameof(Vector3.x)],
      y = (float)obj[nameof(Vector3.y)],
      z = (float)obj[nameof(Vector3.z)]
    };

    public JObject ToJObject() => new() {
      [nameof(Inventory)] = new JArray(Inventory.Select(x => x.ToJObject())),
      [nameof(Asteroids)] = new JArray(Asteroids.Select(a => a.ToJObject())),
      [nameof(Flags)] = new JArray(Flags),
      [nameof(LocationId)] = LocationId,
      [nameof(AsteroidId)] = AsteroidId,
      [nameof(StationId)] = StationId,
      [nameof(PlayerPosition)] = Vector3ToJObject(PlayerPosition)
    };

    public static SaveData FromJObject(JObject json) => new() {
      Inventory = json[nameof(Inventory)]?.Select(t => CraftLib.Item.FromJObject((JObject)t)).ToList() ?? new(),
      Asteroids = json[nameof(Asteroids)]?.Select(t => Asteroid.FromJObject((JObject)t)).ToList() ?? new(),
      Flags = json[nameof(Flags)]?.Select(t => (string)t).ToHashSet() ?? new(),
      LocationId = (string)(json[nameof(LocationId)] ?? string.Empty),
      AsteroidId = json[nameof(AsteroidId)].Value<int>(),
      StationId = json[nameof(StationId)].Value<int>(),
      PlayerPosition = JObjectToVector3((JObject)json[nameof(PlayerPosition)])
    };

    public object Clone() => new SaveData {
      Inventory = Inventory.Select(i => i.Clone() as CraftLib.Item).ToList(),
      Asteroids = Asteroids.Select(a => a.Clone() as Asteroid).ToList(),
      Flags = new HashSet<string>(Flags),
      LocationId = LocationId,
      AsteroidId = AsteroidId,
      StationId = StationId,
      PlayerPosition = PlayerPosition
    };
  }
}
