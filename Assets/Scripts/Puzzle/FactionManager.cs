using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
  public enum FactionType {
    Workers,
    Smugglers,
    Security
  }

  public class FactionManager : MonoBehaviour {
    public static FactionManager Instance;
    private Dictionary<FactionType, Faction> factions;


    private void Awake() {
      if (Instance == null) {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeFactions();
      }
      else {
        Destroy(gameObject);
      }
    }

    private void Start() {
      InitializeFactions();
    }

    private void InitializeFactions() {
      factions = new Dictionary<FactionType, Faction>() {
        { FactionType.Workers, new Faction("Workers", 0) },
        { FactionType.Smugglers, new Faction("Smugglers", 0) },
        { FactionType.Security, new Faction("Security", 0) }
      };
    }

    public void UpdateRelationship(FactionType factionType, int amount) {
      if (factions.ContainsKey(factionType)) {
        factions[factionType].ChangeRelationship(amount);
      }
      else {
        Debug.LogWarning($"Faction {factionType} not found!");
      }
    }

    public int GetFactionRelationship(FactionType factionType) {
      return factions.ContainsKey(factionType) ? factions[factionType].Relationship : -200;
    }
  }
}
