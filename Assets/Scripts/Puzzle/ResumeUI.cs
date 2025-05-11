using TMPro;
using UnityEngine;

namespace Puzzle {
  public class ResumeUI : MonoBehaviour {
    [SerializeField] private TMP_Text questName;
    [SerializeField] private TMP_Text questDesc;
    [SerializeField] private TMP_Text questStep;

    [SerializeField] private GameObject Resume;
    [SerializeField] private GameObject mapPrefab;

    private static ResumeUI instance;

    private void OnEnable() { }

    private void Awake() {
      if (instance == null) {
        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("Resume instance created: " + gameObject.name);
      }
      else if (instance != this) {
        Debug.Log("Duplicate Resume instance destroyed: " + gameObject.name);
        Destroy(gameObject);
      }
      QuestEvents.OnQuestNameUpdated += SetQuestName;
      QuestEvents.OnQuestDescriptionUpdated += SetQuestDescription;
      //QuestEvents.OnQuestStepUpdated += SetQuestStep;

      if (FindFirstObjectByType<MapUI>() == null) {
        Instantiate(mapPrefab, transform);
      }

      Switch(false);
    }

    private void Start() {
      GameController.instance.OnResumeOpen += Switch;
    }

    private void Switch(bool val) {
      Resume.SetActive(val);
    }

    private void SetQuestName(string _questName) {
      questName.text = _questName;
    }

    private void SetQuestDescription(string _questDesc) {
      questDesc.text = _questDesc;
    }

    private void SetQuestStep(string _questStep) {
      questStep.text = _questStep;
    }
  }
}
