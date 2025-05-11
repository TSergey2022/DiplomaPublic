using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
  [CreateAssetMenu(fileName = "QuestData", menuName = "Quest/QuestData")]
  public class Quest : ScriptableObject {
    public string Title;
    public List<FactionImpact> factionImpacts;
    public List<QuestStep> steps = new();
  }

  [System.Serializable]
  public class QuestStep {
    public QuestManager.ObjectiveType stepGoal;
    public string stepDescription;
    public string objectiveID;
  }

  [System.Serializable]
  public class FactionImpact {
    public FactionType faction;
    public int relationshipChange;
  }
}
