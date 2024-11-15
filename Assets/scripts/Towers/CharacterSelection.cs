using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterSelectionManager : MonoBehaviour
{
    public Button addTowerButton;
    public Button cancelPlacementButton;
    public GameObject towerPrefab;

    private GameObject currentTower;
    private bool isPlacingTower = false;
    private bool cancelPressed = false;
    private int towerCost = 1;
    private CurrencyManager currencyManager;

    private PlayerInputs playerInputs;

    private void Awake()
    {
        // Initialize the PlayerInputs instance
        playerInputs = new PlayerInputs();
    }

    void Start()
    {
        currencyManager = GameObject.FindGameObjectWithTag("Master").GetComponent<CurrencyManager>();
        // Added listeners to buttons to handle tower placement and cancellation
        addTowerButton.onClick.AddListener(StartTowerPlacement);
        cancelPlacementButton.onClick.AddListener(CancelTowerPlacement);

        // Add input action listeners
        playerInputs.TowerPlacement.StartPlacement.performed += context => StartTowerPlacement();
        playerInputs.TowerPlacement.CancelPlacement.performed += context => CancelTowerPlacement();
        playerInputs.TowerPlacement.PlaceTower.performed += context => PlaceTower();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    private void StartTowerPlacement()
    {
        // Prevents starting tower placement if it's already in progress
        if (isPlacingTower)
        {
            Debug.Log("Already placing a tower.");
            return;
        }

        isPlacingTower = true;

        // Instantiates the tower if there isn't one currently being placed
        if (currentTower == null)
        {
            currentTower = Instantiate(towerPrefab); // Creates the tower
            currentTower.transform.localScale = Vector3.one; // Sets scale to 1

            // Disable the tower's behavior while placing it
            TowerBehaviour towerBehaviour = currentTower.GetComponent<TowerBehaviour>();
            if (towerBehaviour != null)
            {
                towerBehaviour.enabled = false;
            }

            // Makes the tower follow the mouse
            FollowMouse();
        }
    }

    private void CancelTowerPlacement()
    {
        // Checks if there is a tower being placed
        if (currentTower != null)
        {
            Debug.Log("Cancelling tower placement. Destroying current tower.");
            Destroy(currentTower);  // Removes the tower 
            currentTower = null;
            Debug.Log("Tower destroyed.");
        }

        isPlacingTower = false;
        cancelPressed = true;
        Debug.Log("Tower placement cancelled.");
    }

    void Update()
    {
        // If the cancel button was pressed, reset the flag and return
        if (cancelPressed)
        {
            cancelPressed = false;
            return;
        }

        if (isPlacingTower && currentTower != null)
        {
            FollowMouse(); // Makes the tower follow the mouse

            // Checks for left mouse click (using the new input system)
            if (playerInputs.TowerPlacement.PlaceTower.triggered)
            {
                // Ensures the click wasn't over a ui element
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Click was on UI, not placing tower.");
                    return;
                }

                if (!isPlacingTower)
                {
                    Debug.Log("Placement was cancelled, no tower will be placed.");
                    return;
                }

                // Places the tower at the clicked location
                PlaceTower();
            }
        }
    }

    private void FollowMouse()
    {
        // Casts a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        // If the ray hits something, move the tower to the hit point
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 newPosition = hit.point;
            newPosition.y = 0f; // Ensures the tower stays on the ground
            currentTower.transform.position = newPosition; // Moves the tower
        }
    }

    private void PlaceTower()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        // Defined layers for the path and the floor
        int pathLayerMask = LayerMask.GetMask("Path");
        int floorLayerMask = LayerMask.GetMask("Floor");

        // Prevents placing the tower on the path
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, pathLayerMask))
        {
            Debug.Log($"Cannot place tower on the path: {hit.collider.gameObject.name}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
            return;
        }

        // Checks if the ray hits the floor
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayerMask))
        {
            // Checks for overlapping towers in the hit area
            Collider[] hitColliders = Physics.OverlapSphere(hit.point, 0.3f);
            foreach (Collider collider in hitColliders)
            {
                if (collider.CompareTag("Tower") && collider.gameObject != currentTower)
                {
                    Debug.Log("Cannot place tower: Another tower is already placed here.");
                    return;  // Exits if there's already a tower in this position
                }
            }

            // Places the tower at the hit point
            Debug.Log($"Tower placed at: {hit.point}");
            currentTower.transform.position = hit.point;
            currentTower.tag = "Tower";
            if (currentTower.transform.position == hit.point)
            {
                if (!currencyManager.SpendCurrency(towerCost))
                {
                    Debug.Log("Not enough currency!");
                    return;
                }
            }

            // Enables the tower's behavior after placing it
            TowerBehaviour towerBehaviour = currentTower.GetComponent<TowerBehaviour>();
            if (towerBehaviour != null)
            {
                towerBehaviour.enabled = true;
            }
            currentTower = null;
            isPlacingTower = false;  // Ends tower placement
        }
        else
        {
            Debug.Log("Raycast didn't hit anything.");
        }
    }
}
