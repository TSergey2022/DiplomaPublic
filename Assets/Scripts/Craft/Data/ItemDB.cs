using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Craft.Data {
  public class ItemDB {
    private readonly Dictionary<string, ItemDB_Entry> db = new();
    private readonly Dictionary<string, Sprite> spriteCache = new();

    public void CacheSprites() {
      foreach (var entry in db.Values) {
        entry.CacheSprite(spriteCache);
      }
    }

    public int Count() {
      return db.Count;
    }

    public void Set(string id, string name, string description, string iconPath, Sprite icon, CraftLib.Item etalon) {
      db[id] = new ItemDB_Entry() {
        Name = name,
        Description = description,
        IconPath = iconPath,
        Icon = icon,
        Etalon = etalon.Clone() as CraftLib.Item,
      };
    }

    public ItemDB_Entry Get(string id) {
      if (!db.TryGetValue(id, out var entry)) {
        throw new KeyNotFoundException($"Item with id {id} not found.");
      }
      return entry;
    }

    public ItemDB_Entry GetNew(string id) {
      if (!db.TryGetValue(id, out var entry)) {
        throw new KeyNotFoundException($"Item with id {id} not found.");
      }
      return entry.Clone() as ItemDB_Entry;
    }

    public JObject ToJObject() {
      return JObject.FromObject(db.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToJObject()));
    }

    public static ItemDB FromJObject(JObject json) {
      var itemDB = new ItemDB();
      foreach (var entry in json.Properties()) {
        itemDB.db[entry.Name] = ItemDB_Entry.FromJObject((JObject)entry.Value);
      }

      return itemDB;
    }
  }
}
