using System;
using System.IO;
using Craft.Data;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Craft.Managers {
  public sealed class SaveManager {
    public SaveData SaveData { get; private set; }

    public static SaveManager Instance { get; private set; }

    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static void Instantiate(bool firstSave) {
      if (Instance == null) {
        Instance = new SaveManager();
        if (firstSave) {
          Instance.InitFirstSave();
        }
        else {
          Instance.LoadFromDisk();
        }
      }
    }

    public void SaveToDisk() {
      try {
        var json = SaveData.ToJObject();
        File.WriteAllText(SaveFilePath, json.ToString());
      }
      catch (Exception e) {
        Debug.LogError(e);
      }
    }

    public void InitFirstSave() {
      var firstSaveJson = JObject.Parse(Resources.Load<TextAsset>("save").text);
      SaveData = SaveData.FromJObject(firstSaveJson);
    }

    public void LoadFromDisk() {
      var saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
      if (File.Exists(saveFilePath)) {
        try {
          var text = File.ReadAllText(saveFilePath);
          var json = JObject.Parse(text);
          SaveData = SaveData.FromJObject(json);
        }
        catch (Exception e) {
          Debug.LogError(e);
        }
      }
      else {
        InitFirstSave();
      }
    }
  }
}
