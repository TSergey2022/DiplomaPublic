using UnityEngine;
using UnityEngine.Tilemaps;

namespace Craft.Data {
  [CreateAssetMenu(fileName = "GeneralData", menuName = "ScriptableObject/GeneralDB")]
  public class GeneralDB : ScriptableObject {
    public GameObject gameManagerPrefab;
    public GameObject musicManagerPrefab;
    public GameObject soundManagerPrefab;
    public GameObject fadeManagerPrefab;
    
    public GameObject playerPrefab;
    
    public Tile[] tiles;
    
    public GameObject puzzleGameControllerPrefab;
    public GameObject puzzleCanvasPrefab;
    public GameObject puzzleQuestManagerPrefab;
  }
}
