using Craft.Asteroids;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Craft.World {
  [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
  public class PlayerScript : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private KeyCode inventoryKey = KeyCode.Tab;
    
    [SerializeField] private GameObject inventoryCanvas;
    
    private Rigidbody2D _rb;
    private InteractableScript _currentInteractable;

    private void Start() {
      _rb = GetComponent<Rigidbody2D>();
      _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update() {
      if (Time.timeScale == 0) return;
      HandleInventory();
      HandleInteractionInput();
      HandleMiningInput();
    }

    private void FixedUpdate() {
      HandleMovement();
    }

    private void HandleInventory() {
      if (Input.GetKeyDown(inventoryKey)) {
        inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
      }
    }

    private void HandleMovement() {
      var moveInput = new Vector2(
        Input.GetAxisRaw("Horizontal"),
        Input.GetAxisRaw("Vertical")
      );

      var movement = moveInput.normalized * moveSpeed;
      _rb.linearVelocity = movement;
    }

    private void HandleInteractionInput() {
      if (Input.GetKeyDown(interactKey) && _currentInteractable) {
        _currentInteractable.Interact();
      }
    }

    private void HandleMiningInput() {
      // if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1)) return;
      // Convert mouse screen position to world position
      var mouseWorldPos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);

      // Perform a 2D raycast at the mouse position
      var hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

      if (!hit.collider) return;
      // Check if the hit collider belongs to a Tilemap
      var tilemap = hit.collider.GetComponentInParent<Tilemap>();
      if (!tilemap) return;
      // Convert world position to tilemap cell position
      var cellPosition = tilemap.WorldToCell(hit.point);
      var asteroid = tilemap.GetComponent<AsteroidScript>();
      asteroid.ShowDurabilityText(cellPosition);
      var damage = 0f;
      if (Input.GetMouseButton(0)) damage = Time.deltaTime * 2f;
      if (Input.GetMouseButton(1)) damage = 10000;
      if (damage > 0) asteroid.DamageTile(cellPosition, damage);
    }

    private void OnTriggerEnter2D(Collider2D other) {
      Debug.Log($"({other.name}) entered");
      if (other.TryGetComponent<InteractableScript>(out var interactable)) {
        _currentInteractable = interactable;
      }
    }

    private void OnTriggerExit2D(Collider2D other) {
      Debug.Log($"({other.name}) exited");
      if (other.GetComponent<InteractableScript>() == _currentInteractable) {
        _currentInteractable = null;
      }
    }
  }
}
