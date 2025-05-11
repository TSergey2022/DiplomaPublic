using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Craft.Data {
  public class ItemDB {
    private readonly Dictionary<string, ItemDB_Entry> _db = new();

    public void CacheSprites() {
      foreach (var entry in _db.Values) {
        entry.CacheSprite();
      }
    }

    public int Count() {
      return _db.Count;
    }

    public void Set(string id, string name, string description, string iconPath, Sprite icon, CraftLib.Item etalon) {
      _db[id] = new ItemDB_Entry() {
        Name = name,
        Description = description,
        IconPath = iconPath,
        Icon = icon,
        Etalon = etalon.Clone() as CraftLib.Item,
      };
    }

    public ItemDB_Entry Get(string id) {
      return _db[id];
    }

    public ItemDB_Entry GetNew(string id) {
      return _db[id].Clone() as ItemDB_Entry;
    }

    public JObject ToJObject() {
      return JObject.FromObject(_db.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToJObject()));
    }

    public static ItemDB FromJObject(JObject json) {
      var itemDB = new ItemDB();
      foreach (var entry in json.Properties()) {
        itemDB._db[entry.Name] = ItemDB_Entry.FromJObject((JObject)entry.Value);
      }

      return itemDB;
    }
  }
}
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
        Etalon = Etalon.Clone() as AsteroidTile
      };
    }

    public static TileDB_Entry FromJObject(JObject json) {
      return new TileDB_Entry() {
        Etalon = AsteroidTile.FromJObject((JObject)json[nameof(Etalon)]),
      };
    }
  }
}
using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Craft.Data {
  // ReSharper disable once InconsistentNaming
  public class ItemDB_Entry : ICloneable {
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public Sprite Icon { get; set; }
    public CraftLib.Item Etalon { get; set; }

    public void CacheSprite() {
      if (!Icon) {
        Icon = Resources.Load<Sprite>(IconPath);
      }
    }

    public JObject ToJObject() {
      return new JObject() {
        [nameof(Name)] = Name,
        [nameof(Description)] = Description,
        [nameof(IconPath)] = IconPath,
        [nameof(Etalon)] = Etalon.ToJObject(),
      };
    }

    public object Clone() {
      return new ItemDB_Entry() {
        Name = Name,
        Description = Description,
        IconPath = IconPath,
        Icon = Icon,
        Etalon = Etalon.Clone() as CraftLib.Item,
      };
    }

    public static ItemDB_Entry FromJObject(JObject json) {
      return new ItemDB_Entry() {
        Name = (string)json[nameof(Name)],
        Description = (string)json[nameof(Description)],
        IconPath = (string)json[nameof(IconPath)],
        Etalon = CraftLib.Item.FromJObject((JObject)json[nameof(Etalon)]),
      };
    }
  }
}
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
      return db[id];
    }

    public TileDB_Entry GetNew(string id) {
      return db[id].Clone() as TileDB_Entry;
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
