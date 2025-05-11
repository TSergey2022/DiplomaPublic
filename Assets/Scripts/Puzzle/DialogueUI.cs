using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle {
  public class DialogueUI : MonoBehaviour {
    public static DialogueUI Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button acceptButton;
    public Button rejectButton;
    public Button proceedButton;
    public event Action OpenDialogue;
    public event Action CloseDialogue;


    private void Awake() {
      Instance = this;
      dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(string text) {
      OpenDialogue?.Invoke();
      dialogueText.text = text;
      dialoguePanel.SetActive(true);

      proceedButton.onClick.AddListener(() => { HideDialoguePanel(); });

      proceedButton.gameObject.SetActive(true);

      acceptButton.gameObject.SetActive(false);
      rejectButton.gameObject.SetActive(false);
    }

    public void GiveQuest(string text, Action onAccept = null, Action onReject = null) {
      OpenDialogue?.Invoke();
      dialogueText.text = text;
      dialoguePanel.SetActive(true);

      acceptButton.onClick.RemoveAllListeners();
      rejectButton.onClick.RemoveAllListeners();

      if (onAccept != null)
        acceptButton.onClick.AddListener(() => {
          onAccept();
          HideDialoguePanel();
        });
      if (onReject != null)
        rejectButton.onClick.AddListener(() => {
          onReject();
          HideDialoguePanel();
        });

      proceedButton.gameObject.SetActive(false);

      acceptButton.gameObject.SetActive(onAccept != null);
      rejectButton.gameObject.SetActive(onReject != null);
    }

    public void HideDialoguePanel() {
      CloseDialogue?.Invoke();
      dialoguePanel.SetActive(false);
    }
  }
}
