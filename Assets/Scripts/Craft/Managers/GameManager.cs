using System;
using System.Collections;
using Craft.Data;
using Craft.UI;
using Craft.World;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Craft.Managers {
  public sealed class GameManager : MonoBehaviour {
    [SerializeField] private GeneralDB generalData;

    [SerializeField] private GameObject craftingCanvas;
    [SerializeField] private GameObject pauseMenuCanvas;

    private GameObject _player;

    private Vector3? _nextPlayerPosition;

    public static GameManager Instance { get; private set; }

    public bool Paused { get; private set; }

    private void Awake() {
      if (Instance == null) {
        Instance = this;
        Init();
        DontDestroyOnLoad(gameObject);
      }
      else {
        Destroy(gameObject);
      }
    }

    private IEnumerator Start() {
      yield return new WaitForSeconds(1);
      FadeManager.Instance.FadeIn();
    }

    private void Init() {
      Instantiate(generalData.musicManagerPrefab);
      Instantiate(generalData.soundManagerPrefab);
      Instantiate(generalData.fadeManagerPrefab);
      Instantiate(generalData.puzzleGameControllerPrefab);
      Instantiate(generalData.puzzleCanvasPrefab);
      Instantiate(generalData.puzzleQuestManagerPrefab);
      
      var itemDBJson = JObject.Parse(Resources.Load<TextAsset>("db/items").text);
      var receiptsDBJson = JObject.Parse(Resources.Load<TextAsset>("db/receipts").text);
      var tilesDBJson = JObject.Parse(Resources.Load<TextAsset>("db/tiles").text);
      CraftManager.Instantiate(itemDBJson, receiptsDBJson);
      AsteroidManager.Instantiate(tilesDBJson, generalData.tiles);
      SaveManager.Instantiate(false);
    }

    private void Update() {
      if (!Input.GetKeyDown(KeyCode.Escape)) return;
      var scene = SceneManager.GetActiveScene();
      if (scene.name is C.SceneNameMainMenu) return;
      if (pauseMenuCanvas.activeSelf) {
        ResumeGame();
      }
      else {
        PauseGame();
      }
    }

#if UNITY_EDITOR
    private void OnApplicationQuit() {
      if (_player != null) {
        SaveManager.Instance.SaveData.PlayerPosition = _player.transform.position;
      }
      SaveManager.Instance.SaveToDisk();
    }

    private void OnDestroy() {
      if (this == Instance) {
        Instance = null;
      }
    }
#endif
    public void SetNextPlayerPos(Vector3 pos) {
      _nextPlayerPosition = pos;
    }

    public void OpenCraftingCanvas(string containerId) {
      craftingCanvas.SetActive(true);
      var craftingCanvasScript = craftingCanvas.GetComponent<CraftingCanvasScript>();
      craftingCanvasScript.SetReceipts(CraftManager.Instance.GetReceipts(containerId));
    }

    public void OpenCraftingCanvas(ReceiptContainerScript receiptContainer) {
      craftingCanvas.SetActive(true);
      var craftingCanvasScript = craftingCanvas.GetComponent<CraftingCanvasScript>();
      craftingCanvasScript.SetReceipts(receiptContainer);
    }

    public void StartNewGame() {
      SaveManager.Instance.InitFirstSave();
      SaveManager.Instance.SaveToDisk();
      // GoToGame();
      LoadSceneWithFader("Ship Garage");
    }

    public void ContinueGame() {
      SaveManager.Instance.LoadFromDisk();
      GoToGame();
    }

    private void GoToGame() {
      _nextPlayerPosition = SaveManager.Instance.SaveData.PlayerPosition;
      if (SaveManager.Instance.SaveData.LocationId == C.SceneNameStation) {
        GoToStation();
      }
      else if (SaveManager.Instance.SaveData.LocationId == C.SceneNameAsteroid) {
        GoToAsteroid(SaveManager.Instance.SaveData.AsteroidId);
      }
      else {
        throw new Exception("Location not found");
      }
    }

    private void SpawnPlayerAction(Scene scene, LoadSceneMode mode) {
      var player = Instantiate(generalData.playerPrefab, _nextPlayerPosition!.Value, Quaternion.identity);
      _nextPlayerPosition = null;
      var mainCamera = Camera.main!;
      mainCamera.transform.parent = player.transform;
      mainCamera.transform.position = player.transform.position + Vector3.back * 10;
      // SceneManager.sceneLoaded -= SpawnPlayerAction;
    }

    private void LoadSceneAndSpawnPlayer(string sceneName) {
      // SceneManager.sceneLoaded += SpawnPlayerAction;
      LoadSceneWithFader(sceneName);
    }

    private string _targetScene;
    private bool _isTransitioning;

    public void LoadSceneWithFader(string sceneName) {
      if (_isTransitioning)
        return;
      _targetScene = sceneName;
      _isTransitioning = true;
      FadeManager.Instance.onFadeOut.AddListener(LoadSceneWithFader_Part1);
      FadeManager.Instance.FadeOut();
    }

    private void LoadSceneWithFader_Part1() {
      FadeManager.Instance.onFadeOut.RemoveListener(LoadSceneWithFader_Part1);
      SceneManager.sceneLoaded += LoadSceneWithFader_Part2;
      SceneManager.LoadScene(_targetScene);
    }

    private void LoadSceneWithFader_Part2(Scene scene, LoadSceneMode mode) {
      SceneManager.sceneLoaded -= LoadSceneWithFader_Part2;
      FadeManager.Instance.FadeIn();
      _isTransitioning = false;
    }

    public void GoToAsteroid(int id) {
      SaveManager.Instance.SaveData.AsteroidId = id;
      LoadSceneAndSpawnPlayer(C.SceneNameAsteroid);
    }

    public void GoToStation() {
      LoadSceneAndSpawnPlayer(C.SceneNameStation);
    }

    public void GoToMainMenu() {
      LoadSceneWithFader(C.SceneNameMainMenu);
    }

    public void ResumeGame() {
      pauseMenuCanvas.SetActive(false);
      Time.timeScale = 1;
    }

    public void PauseGame() {
      Time.timeScale = 0;
      pauseMenuCanvas.SetActive(true);
    }
  }
}
