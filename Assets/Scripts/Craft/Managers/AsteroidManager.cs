using System;
using Craft.Asteroids;
using Craft.Data;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Craft.Managers {
  public class AsteroidManager {
    public static AsteroidManager Instance { get; private set; }
    private TileDB tileDB;

    public static void Instantiate(JObject json, Tile[] tiles) {
      if (Instance == null) {
        Instance = new AsteroidManager {
          tileDB = TileDB.FromJObject(json)
        };
        Instance.tileDB.UpdateTiles(tiles);
      }
    }
    
    public TileDB_Entry GetTile(string id) {
      return tileDB.GetNew(id);
    }
    
    public void FillAsteroid(Asteroid asteroid, int oreTier) {
      var center = new Vector2Int(Random.Range(-1000, 1000), Random.Range(-1000, 1000));
      FillAsteroid(asteroid, oreTier, center);
    }
    
    public void FillAsteroid(Asteroid asteroid, int oreTier, Vector2Int center) {
      var startTime = DateTime.Now;
      Debug.Log($"[{nameof(FillAsteroid)}] Start. ore={oreTier}");
      var fillTiles = asteroid.Tiles;
      const int radiusOuter = 50;
      const int radiusInner = radiusOuter - 5;
      var shellTile = GetTile("shell").Etalon;
      var softTile = GetTile("soft").Etalon;
      var oreWeakTileId = oreTier switch {
        1 => "copper_ore",
        2 => "gold_ore",
        3 => "cobalt_ore",
        _ => ""
      };
      var oreStrongTileId = oreTier switch {
        1 => "iron_ore",
        2 => "platinum_ore",
        3 => "titanium_ore",
        _ => ""
      };
      var oreFuelTileId = oreTier switch {
        1 => "coal",
        2 => "solid_petroleum",
        3 => "lavastone",
        _ => ""
      };
      var oreWeakTile = GetTile(oreWeakTileId).Etalon;
      var oreStrongTile = GetTile(oreStrongTileId).Etalon;
      var fuelTile = GetTile(oreFuelTileId).Etalon;
      for (var x = -radiusOuter; x < radiusOuter; x++) {
        for (var y = -radiusOuter; y < radiusOuter; y++) {
          var m = new Vector2(x, y).magnitude;
          if (m > radiusOuter) continue;
          if (m > radiusInner) {
            fillTiles[new Vector3Int(x, y, 0)] = shellTile.Clone() as AsteroidTile;
          }
          else {
            var noise = 0.5f - AsteroidNoise(x, y, 4f / radiusOuter, center);
            if (Mathf.Abs(noise) < 0.05f) { }
            else if (Mathf.Abs(noise) < 0.2f) {
              fillTiles[new Vector3Int(x, y, 0)] = softTile.Clone() as AsteroidTile;
            }
            else if (Mathf.Abs(noise) < 0.3f) {
              fillTiles[new Vector3Int(x, y, 0)] = fuelTile.Clone() as AsteroidTile;
            }
            else if (noise >= 0.3f) {
              fillTiles[new Vector3Int(x, y, 0)] = oreWeakTile.Clone() as AsteroidTile;
            }
            else if (noise <= -0.3f) {
              fillTiles[new Vector3Int(x, y, 0)] = oreStrongTile.Clone() as AsteroidTile;
            }
          }
        }
      }

      Debug.Log($"[{nameof(FillAsteroid)}] Finished. {(DateTime.Now - startTime).TotalMilliseconds} ms.");
    }

    private static float AsteroidNoise(int x, int y, float frequency = 1f, Vector2Int center = default) {
      var noise = Mathf.PerlinNoise(center.x + x * frequency, center.y + y * frequency);
      noise = Mathf.Clamp01(noise);
      return noise;
    }
  }
}
