using System;
using System.Collections.Generic;
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

    public void CacheSprite(Dictionary<string, Sprite> spriteCache) {
      if (Icon || string.IsNullOrEmpty(IconPath)) return;
      if (!spriteCache.TryGetValue(IconPath, out var cachedIcon)) {
        cachedIcon = Resources.Load<Sprite>(IconPath);
        spriteCache[IconPath] = cachedIcon;
      }
      Icon = cachedIcon;
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
        Etalon = Etalon?.Clone() as CraftLib.Item, // Проверка на null
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
