using System;
using System.Collections.Generic;
using System.Linq;
using Craft.Asteroids;
using Newtonsoft.Json.Linq;
using UnityEngine.Tilemaps;

namespace Craft.Data {
  public class TileDB {
    private Dictionary<string, TileDB_Entry> db = new();

    public void UpdateTiles(IEnumerable<Tile> tiles) {
      foreach (var tile in tiles) {
        if (db.TryGetValue(tile.name, out var entry)) {
          entry.Tile = tile;
        }
      }
    }

    public int Count() {
      return db.Count;
    }

    public void Set(string id, Tile tile, AsteroidTile etalon) {
      db[id] = new TileDB_Entry() {
        Tile = tile,
        Etalon = etalon.Clone() as AsteroidTile,
      };
    }

    public TileDB_Entry Get(string id) {
      if (!db.TryGetValue(id, out var entry)) {
        throw new KeyNotFoundException($"Tile with id {id} not found.");
      }
      return entry;
    }

    public TileDB_Entry GetNew(string id) {
      if (!db.TryGetValue(id, out var entry)) {
        throw new KeyNotFoundException($"Tile with id {id} not found.");
      }
      return entry.Clone() as TileDB_Entry;
    }

    public JObject ToJObject() {
      return JObject.FromObject(db.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToJObject()));
    }

    public static TileDB FromJObject(JObject json) {
      return new TileDB() {
        db = json
          .Properties()
          .ToDictionary(
            kvp => kvp.Name,
            kvp => TileDB_Entry.FromJObject((JObject)kvp.Value)
          )
      };
    }
  }
}
