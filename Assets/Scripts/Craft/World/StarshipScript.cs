using System;
using Craft.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Craft.World {
  public class StarshipScript : MonoBehaviour {
    enum StarshipMode { Invalid, GoToAsteroid, GoToStation }
    
    [SerializeField] private GameObject innerCanvas;
    [SerializeField] private GameObject asteroidButtons;
    [SerializeField] private GameObject stationButtons;
    [SerializeField] private Button asteroid1Button;
    [SerializeField] private Button asteroid2Button;
    [SerializeField] private Button asteroid3Button;
    [SerializeField] private StarshipMode starshipMode = StarshipMode.Invalid;
    
    private void Start() {
      UpdateButtons();
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
      if (other.gameObject.CompareTag(C.GAMEOBJECT_TAG_PLAYER)) {
        innerCanvas.SetActive(true);
        UpdateButtons();
      }
    }
    private void OnTriggerExit2D(Collider2D other) {
      if (other.gameObject.CompareTag(C.GAMEOBJECT_TAG_PLAYER)) {
        innerCanvas.SetActive(false);
      }
    }

    private void UpdateButtons() {
      if (starshipMode == StarshipMode.Invalid) {
        throw new Exception("Invalid starship mode");
      }
      if (starshipMode == StarshipMode.GoToAsteroid) {
        var flags = SaveManager.Instance.SaveData.Flags;
        asteroid1Button.interactable = flags.Contains("asteroid1");
        asteroid2Button.interactable = flags.Contains("asteroid2");
        asteroid3Button.interactable = flags.Contains("asteroid3");
      }
      asteroidButtons.SetActive(starshipMode == StarshipMode.GoToAsteroid);
      stationButtons.SetActive(starshipMode == StarshipMode.GoToStation);
    }
  }
}