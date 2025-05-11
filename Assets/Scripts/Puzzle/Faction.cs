using UnityEngine;

namespace Puzzle {
  [System.Serializable]
  public class Faction {
    public string Name;
    public int Relationship;

    public Faction(string name, int initialRelationship) {
      Name = name;
      Relationship = initialRelationship;
    }

    public void ChangeRelationship(int amount) {
      Relationship = Mathf.Clamp(Relationship + amount, -100, 100);
    }
  }
}
