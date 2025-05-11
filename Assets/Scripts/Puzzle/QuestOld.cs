namespace Puzzle {
    public class QuestOld {
        /*public string QuestID { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public QuestStatus Status { get; private set; }
    public List<QuestObjective> Objectives { get; private set; }
    public int RewardPoints { get; private set; }

    private int currentObjectiveIndex = 0; 

    public QuestOld(string questID, string title, string description, List<QuestObjective> objectives, int rewardPoints = 0) {
        QuestID = questID;
        Title = title;
        Description = description;
        Status = QuestStatus.Inactive;
        Objectives = objectives;
        RewardPoints = rewardPoints;
    }

    public void ActivateQuest() {
        if (Status == QuestStatus.Inactive) {
            Status = QuestStatus.Active;
            Debug.Log($"Quest '{Title}' activated!");
            foreach (var objective in Objectives) {
                //Debug.Log($"Objective added: {objective.Type} - Target: {objective.TargetID}");
            }
        } else {
            Debug.Log($"Quest '{Title}' is already active or completed.");
        }
    }

    public void CompleteObjective(string targetID) {
        if (Status != QuestStatus.Active) return;

        QuestObjective currentObjective = Objectives[currentObjectiveIndex];
        if (currentObjective.TargetID == targetID && !currentObjective.IsCompleted) {
            currentObjective.CompleteObjective();
            Debug.Log($"Objective '{currentObjective.Type}' with target '{targetID}' completed!");

            currentObjectiveIndex++;

            if (currentObjectiveIndex > Objectives.Count) {
                CompleteQuest();
            }
            else
            {
                //QuestEvents.OnQuestStepUpdated?.Invoke(Objectives[currentObjectiveIndex].Description);
            }
        } else {
            Debug.Log($"Objective '{targetID}' cannot be completed yet. Complete previous steps first.");
        }
    }

    private void CompleteQuest() {
        Status = QuestStatus.Completed;
        Debug.Log($"Quest '{Title}' completed! Reward: {RewardPoints} points.");
    }*/
    }
}
