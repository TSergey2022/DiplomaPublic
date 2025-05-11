using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
  public enum QuestStatus {
    Inactive,
    Active,
    Completed
  }

  public class QuestManager : MonoBehaviour {
    public enum ObjectiveType {
      InteractWithObject,
      InteractWithNPC,
      CompletePuzzle
    }

    [SerializeField] public Quest currentQuest;
    public QuestStep currentQuestStep;
    public List<Quest> activeQuests = new();
    private int currentQuestStepInd;

    public static QuestManager instance;

    private void Awake() {
      if (instance == null) {
        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("QuestManager instance created: " + gameObject.name);
      }
      else if (instance != this) {
        Debug.Log("Duplicate QuestManager instance destroyed: " + gameObject.name);
        Destroy(gameObject);
      }
    }

    private void OnEnable() {
      QuestEvents.OnObjectInteracted += HandleObjectInteraction;
      QuestEvents.OnNPCInteracted += HandleNPCInteraction;
      QuestEvents.OnPuzzleCompleted += HandlePuzzleCompletion;
      //QuestEvents.OnQuestStepUpdated += QuestStepFinished;
    }


    private void OnDisable() {
      QuestEvents.OnObjectInteracted -= HandleObjectInteraction;
      QuestEvents.OnNPCInteracted -= HandleNPCInteraction;
      QuestEvents.OnPuzzleCompleted -= HandlePuzzleCompletion;
      //QuestEvents.OnQuestStepUpdated -= QuestStepFinished;
    }

    public void AcceptQuest(Quest newQuest) {
      currentQuest = newQuest;
      currentQuestStep = currentQuest.steps[0];
      QuestEvents.OnQuestDescriptionUpdated?.Invoke(currentQuest.Title);
      QuestEvents.OnQuestStepUpdated?.Invoke(currentQuestStep.stepDescription);
    }

    public void QuestStepFinished() {
      currentQuestStepInd++;
      if (currentQuestStepInd <= currentQuest.steps.Count - 1) {
        currentQuestStep = currentQuest.steps[currentQuestStepInd];
        QuestEvents.OnQuestDescriptionUpdated?.Invoke(currentQuestStep.stepDescription);
      }
      else
        FinishQuest();
    }

    public void FinishQuest() {
      Debug.Log(
        $"Отношения до: Рабочие {FactionManager.Instance.GetFactionRelationship(FactionType.Workers)}, Контрабандисты {FactionManager.Instance.GetFactionRelationship(FactionType.Smugglers)}, СБ {FactionManager.Instance.GetFactionRelationship(FactionType.Security)}");
      foreach (var impact in currentQuest.factionImpacts) {
        FactionManager.Instance.UpdateRelationship(impact.faction, impact.relationshipChange);
      }

      Debug.Log(
        $"Отношения после: Рабочие {FactionManager.Instance.GetFactionRelationship(FactionType.Workers)}, Контрабандисты {FactionManager.Instance.GetFactionRelationship(FactionType.Smugglers)}, СБ {FactionManager.Instance.GetFactionRelationship(FactionType.Security)}");

      currentQuest = null;
    }

    public void AddQuest(Quest quest) {
      /*activeQuests.Add(quest);
  quest.ActivateQuest();

  QuestEvents.OnQuestNameUpdated?.Invoke(quest.Title);
  QuestEvents.OnQuestDescriptionUpdated?.Invoke(quest.Description);
  QuestEvents.OnQuestStepUpdated?.Invoke(quest.Objectives[0].Description);*/
    }

    private void HandleObjectInteraction(string targetID) {
      if (currentQuestStep.stepGoal == ObjectiveType.InteractWithObject) {
        if (targetID == currentQuestStep.objectiveID)
          QuestStepFinished();
      }
    }

    private void HandleNPCInteraction(string npcID) {
      if (currentQuestStep.stepGoal == ObjectiveType.InteractWithNPC) {
        if (npcID == currentQuestStep.objectiveID)
          QuestStepFinished();
      }
    }

    private void HandlePuzzleCompletion(string puzzleID) {
      if (currentQuestStep.stepGoal == ObjectiveType.CompletePuzzle) {
        if (puzzleID == currentQuestStep.objectiveID)
          QuestStepFinished();
      }
    }

    private void UpdateQuestProgress(ObjectiveType type, string targetID) {
      /*  foreach (var quest in activeQuests) {
      if (quest.Status == QuestStatus.Active) {
          quest.CompleteObjective(targetID);
      }
  }*/
    }

    private void Start() {
      /*QuestManager questManager = FindFirstObjectByType<QuestManager>();

Quest quest1 = new Quest(
  questID: "quest001",
  title: "Сядь в корабль",
  description: "Сядь в корабль и лети отсюда.",
  objectives: new List<QuestObjective> {
      new QuestObjective(ObjectiveType.InteractWithObject, "Ship", "Нужно пойти и сесть в корабль"),
      new QuestObjective(ObjectiveType.InteractWithObject, "Guy", "Не взлетело. Надо пойти обсудить с человеком."),
      new QuestObjective(ObjectiveType.CompletePuzzle, "FirstPuzzle", "Надо влезть."),
      new QuestObjective(ObjectiveType.InteractWithObject, "Guy", "Че теперь расскажет."),
  },
  rewardPoints: 100
);

  Quest quest2 = new Quest(
  questID: "quest001",
  title: "Сядь в корабль",
  description: "Сядь в корабль и лети отсюда.",
  objectives: new List<QuestObjective> {
      new QuestObjective(ObjectiveType.InteractWithObject, "SB001", "Нужно пойти перетереть с СБ-шником"),
      new QuestObjective(ObjectiveType.InteractWithObject, "Ship", "Ну пришло время лететь искать че он там от меня хочет"),
      new QuestObjective(ObjectiveType.CompletePuzzle, "SB001", "Ну я принес ему, посмотрим что теперь скажет"),
  },
  rewardPoints: 100
);

questManager.AddQuest(quest1);
questManager.AddQuest(quest2);*/
    }
  }
}
