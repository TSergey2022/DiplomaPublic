using System;
using Craft.Managers;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Craft.Asteroids {
  [RequireComponent(typeof(Tilemap))]
  public class AsteroidScript : MonoBehaviour {
    private Asteroid Asteroid { get; set; }
    
    private Tilemap tilemap;
    [SerializeField] private Text damageText;

    private float damageTextTimer;
    
#if UNITY_EDITOR
    [SerializeField] private bool forceSpawnCustomAsteroid;
    [SerializeField] private int customAsteroidId;
#endif
    
    private void Start() {
      tilemap = GetComponent<Tilemap>();
      var asteroidId = SaveManager.Instance.SaveData.AsteroidId;
#if UNITY_EDITOR
      if (forceSpawnCustomAsteroid) {
        asteroidId = customAsteroidId;
      }
#endif
      for (var i = SaveManager.Instance.SaveData.Asteroids.Count; i < asteroidId; i++) {
        var asteroid = new Asteroid();
        SaveManager.Instance.SaveData.Asteroids.Add(asteroid);
      }
      Asteroid = SaveManager.Instance.SaveData.Asteroids[asteroidId - 1];
      if (Asteroid.IsEmpty()) {
        AsteroidManager.Instance.FillAsteroid(Asteroid, asteroidId);
      }
      foreach (var (position, asteroidTile) in Asteroid.Tiles) {
        var tile = AsteroidManager.Instance.GetTile(asteroidTile.Id).Tile;
        tilemap.SetTile(position, tile);
      }
    }

    private void Update() {
      if (GameManager.Instance.Paused) return;
      damageTextTimer -= Time.deltaTime;
      damageText.enabled = damageTextTimer > 0f;
    }
  
    public void CreateTile(string id, Vector3Int position) {
      var entry = AsteroidManager.Instance.GetTile(id);
      Asteroid.Tiles[position] = entry.Etalon;
      tilemap.SetTile(position, entry.Tile);
    }

    public void ShowDurabilityText(Vector3Int position) {
      var tileExists = Asteroid.Tiles.ContainsKey(position);
      damageText.enabled = tileExists;
      if (tileExists) {
        damageTextTimer = 2f;
        var tile = Asteroid.Tiles[position];
        damageText.transform.position = tilemap.CellToWorld(position);
        damageText.text = Mathf.FloorToInt(tile.Durability * 10).ToString();
      }
    }
  
    public void DamageTile(Vector3Int position, float damage = 1.0f) {
      try {
        var tile = Asteroid.Tiles[position];
        tile.Durability -= damage;
        if (tile.Durability <= 0) {
          damageTextTimer = 0;
          damageText.enabled = false;
          Asteroid.Tiles.Remove(position);
          tilemap.SetTile(position, null);
          if (tile.Drop != null) {
            SaveManager.Instance.SaveData.Inventory.Add(tile.Drop);
          }
        }
      } catch (Exception e) {
        Debug.LogError(e);
      }
    }
  }
}
