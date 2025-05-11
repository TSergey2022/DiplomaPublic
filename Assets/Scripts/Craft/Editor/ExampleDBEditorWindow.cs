using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Craft.Asteroids;
using Newtonsoft.Json.Linq;
using Craft.CraftLib;
using Craft.Data;
using Craft.Processors;
using Craft.Requirements;

namespace Craft.Editor {
  public class ExampleDBEditorWindow : EditorWindow {
    [MenuItem("EditorWindow/ExampleDBEditorWindow")]
    public static void ShowWindow() {
      GetWindow<ExampleDBEditorWindow>("ExampleDBEditorWindow");
    }

    private void OnGUI() {
      if (GUILayout.Button("Generate")) {
        GenFirstSave();
        GenItemDB();
        GenReceiptDB();
        GenTileDB();
      }
    }

    private void GenFirstSave() {
      var saveData = new SaveData();

      saveData.LocationId = C.SceneNameStation;
      saveData.Asteroids = new List<Asteroid> { new(), new(), new() };
      saveData.Flags.Add("asteroid1");

      var json = saveData.ToJObject();

      var filePath = Path.Combine(Application.dataPath, "Resources", "save.json");
      File.WriteAllText(filePath, json.ToString());

      AssetDatabase.Refresh();

      Debug.Log($"[{nameof(GenFirstSave)}] Success");
    }
    
    private void GenItemDB() {
      var itemDB = new ItemDB();
      
      itemDB.Set("item1", "Item 1", "blah-blah", "Resoucre/Sprites/32", null, new Item() {
        Id = "item1",
        Tags = new List<Tag>() {
          new() { Id = "quantity", Value = 5 },
          new() { Id = "ore" },
        }
      });

      var json = itemDB.ToJObject();

      var filePath = Path.Combine(Application.dataPath, "Resources", "db", "items.json");
      File.WriteAllText(filePath, json.ToString());

      AssetDatabase.Refresh();

      Debug.Log($"[{nameof(GenItemDB)}] Success");
    }

    private void GenReceiptDB() {
      var receiptsDB = new Dictionary<string, List<Receipt>>();
      receiptsDB.Add("station1", new List<Receipt>() {
        new() {
          Id = "station1.receipt1",
          Inputs = new List<InputSlot>() {
            new() { Name = "station1.slot1", Requirement = new IdRequirement() { RequiredId = "item1"} }
          },
          Output = new OutputSlot() {
            Name = "station1.output",
            Processor = new SimpleProcessor() {
              OutputItems = new List<Item>() {
                new() {
                  Id = "item2",
                  Tags = new List<Tag>() {
                    new() { Id = "quantity", Value = 10 },
                    new() { Id = "bar" },
                  }
                }
              }
            }
          }
        }
      });
      
      var json = new JObject();
      foreach (var kvp in receiptsDB) {
        var receiptsArray = new JArray();
        foreach (var receipt in kvp.Value) {
          receiptsArray.Add(receipt.ToJObject());
        }
        json[kvp.Key] = receiptsArray;
      }

      var filePath = Path.Combine(Application.dataPath, "Resources", "db", "receipts.json");
      File.WriteAllText(filePath, json.ToString());

      AssetDatabase.Refresh();

      Debug.Log($"[{nameof(GenReceiptDB)}] Success");
    }
    
    private void GenTileDB() {
      var tileDB = new TileDB();
      
      tileDB.Set("tile1", null, new AsteroidTile() {
        Id = "tile1",
        Durability = 2,
        Drop = new Item() {
          Id = "item1",
          Tags = new List<Tag>() {
            new() { Id = "quantiry", Value = 1},
            new() { Id = "ore" }
          }
        }
      });
      tileDB.Set("tile2", null, new AsteroidTile() {
        Id = "tile2",
        Durability = 1,
        Drop = new Item() {
          Id = "item3",
          Tags = new List<Tag>() {
            new() { Id = "quantiry", Value = 5},
            new() { Id = "ore" }
          }
        }
      });

      var json = tileDB.ToJObject();

      var filePath = Path.Combine(Application.dataPath, "Resources", "db", "tiles.json");
      File.WriteAllText(filePath, json.ToString());

      AssetDatabase.Refresh();

      Debug.Log($"[{nameof(GenTileDB)}] Success");
    }
    
  }
}
