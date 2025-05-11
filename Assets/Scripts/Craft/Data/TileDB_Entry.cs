using System;
using Craft.Asteroids;
using Newtonsoft.Json.Linq;
using UnityEngine.Tilemaps;

namespace Craft.Data {
  // ReSharper disable once InconsistentNaming
  public class TileDB_Entry : ICloneable {
    public Tile Tile { get; set; }
    public AsteroidTile Etalon { get; set; }

    public JObject ToJObject() {
      return new JObject() {
        [nameof(Etalon)] = Etalon.ToJObject(),
      };
    }

    public object Clone() {
      return new TileDB_Entry {
        Tile = Tile,
        Etalon = Etalon?.Clone() as AsteroidTile, // Проверка на null
      };
    }

    public static TileDB_Entry FromJObject(JObject json) {
      return new TileDB_Entry() {
        Etalon = AsteroidTile.FromJObject((JObject)json[nameof(Etalon)]),
      };
    }
  }
}
