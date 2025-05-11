using System;

namespace Puzzle {
  public static class QuestEvents {
    public static Action<string> OnObjectInteracted;
    public static Action<string> OnNPCInteracted;
    public static Action<string> OnPuzzleCompleted;
    public static Action<string> OnQuestNameUpdated;
    public static Action<string> OnQuestDescriptionUpdated;
    public static Action<string> OnQuestStepUpdated;
  }
}
