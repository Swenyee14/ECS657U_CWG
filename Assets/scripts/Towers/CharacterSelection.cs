using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class CharacterSelectionManager : MonoBehaviour
{
    public Button addTowerButton1;
    public Button addTowerButton2;
    public Button addTowerButton3;
    public Button cancelPlacementButton;

    public GameObject[] towerPrefabs; // Array to hold multiple tower prefabs

    private GameObject selectedTowerPrefab; // Currently selected tower prefab
    private GameObject currentTower;
    private bool isPlacingTower = false;
    private bool cancelPressed = false;
    private int[] towerCosts = { 1, 5, 10 };
    public TextMeshProUGUI addTowerButton1Text;
    public TextMeshProUGUI addTowerButton2Text;
    public TextMeshProUGUI addTowerButton3Text;
    private CurrencyManager currencyManager;

    private PlayerInputs playerInputs;

    // Stores the calculated tower position
    private Vector3 towerPlacementPosition;
    private int[] towerPlacementLimits = { 7, 4, 2 }; // Maximum placements for each tower
    private int[] towerPlacementCounts;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        // Map keyboard shortcuts for selecting towers
        playerInputs.TowerSelection.SelectTower1.performed += context => SelectTower(0);
        playerInputs.TowerSelection.SelectTower2.performed += context => SelectTower(1);
        playerInputs.TowerSelection.SelectTower3.performed += context => SelectTower(2);

        playerInputs.TowerPlacement.CancelPlacement.performed += context => CancelTowerPlacement();
    }

    void Start()
    {
        towerPlacementCounts = new int[towerPlacementLimits.Length];
        currencyManager = GameObject.FindGameObjectWithTag("Master").GetComponent<CurrencyManager>();

        // Assign listeners for tower selection buttons
        addTowerButton1.onClick.AddListener(() => SelectTower(0));
        addTowerButton2.onClick.AddListener(() => SelectTower(1));
        addTowerButton3.onClick.AddListener(() => SelectTower(2));

        // Assign listener for cancel button
        cancelPlacementButton.onClick.AddListener(CancelTowerPlacement);

        // Map input actions
        playerInputs.TowerPlacement.CancelPlacement.performed += context => CancelTowerPlacement();
        playerInputs.TowerPlacement.PlaceTower.performed += context => PlaceTower();

        UpdateTowerButtonTexts();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    // Update the button text for the towers if the text component exists
    private void UpdateTowerButtonTexts()
    {
        if (addTowerButton1Text != null)
            addTowerButton1Text.text = $"Basic Tower ({towerCosts[0]} cost)";
        {
            addTowerButton1Text.text = $"Basic Tower ({towerCosts[0]} cost)\n" +
                                       $"{towerPlacementCounts[0]}/{towerPlacementLimits[0]} placed";
        }
        if (addTowerButton2Text != null)
            addTowerButton2Text.text = $"Sniper Tower ({towerCosts[1]} cost)";
        {
            addTowerButton2Text.text = $"Sniper Tower ({towerCosts[1]} cost)\n" +
                                       $"{towerPlacementCounts[1]}/{towerPlacementLimits[1]} placed";
        }
        if (addTowerButton3Text != null)
            addTowerButton3Text.text = $"Bomb Tower ({towerCosts[2]} cost)";
        {
            addTowerButton3Text.text = $"Bomb Tower ({towerCosts[2]} cost)\n" +
                                       $"{towerPlacementCounts[2]}/{towerPlacementLimits[2]} placed";
        }
    }

    private void SelectTower(int towerIndex)
    {

        // Validate the tower index to ensure it's within range
        if (towerIndex < 0 || towerIndex >= towerPrefabs.Length)
        {
            Debug.LogError("Invalid tower index selected.");
            return;
        }

        // Check if the selected tower has reached its placement limit
        if (towerPlacementCounts[towerIndex] >= towerPlacementLimits[towerIndex])
        {
            Debug.Log($"Tower {towerIndex + 1} has reached its placement limit.");
            return;
        }

        // Cancel any ongoing tower placement if applicable
        if (isPlacingTower)
        {
            CancelTowerPlacement();
        }

        // Hide the tower menu if it is currently active
        HideTowerMenuIfActive();

        // Set the selected tower prefab and start its placement process
        selectedTowerPrefab = towerPrefabs[towerIndex];
        StartTowerPlacement();
        Debug.Log($"Tower {towerIndex + 1} selected: {selectedTowerPrefab.name}");
    }

    private void StartTowerPlacement()
    {
        // Ensure no tower placement is already in progress
        if (isPlacingTower)
        {
            Debug.Log("Already placing a tower.");
            return;
        }

        // Validate that a tower prefab has been selected
        if (selectedTowerPrefab == null)
        {
            Debug.LogError("No tower prefab selected!");
            return;
        }

        isPlacingTower = true;

        // Instantiate the selected tower and reset its scale
        currentTower = Instantiate(selectedTowerPrefab);
        currentTower.transform.localScale = Vector3.one;

        // Disable the tower's behavior script until placement is confirmed
        TowerBehaviour towerBehaviour = currentTower.GetComponent<TowerBehaviour>();
        if (towerBehaviour != null)
        {
            towerBehaviour.enabled = false;
        }

        FollowMouse();
    }

    //cancels tower placement
    private void CancelTowerPlacement()
    {
        if (currentTower != null)
        {
            Destroy(currentTower);
            currentTower = null;
            Debug.Log("Tower placement cancelled. Tower destroyed.");
        }
        else
        {
            Debug.Log("No active tower to cancel.");
        }
        isPlacingTower = false;
    }

    // hides tower menu
    private void HideTowerMenuIfActive()
    {
        if (TowerSelector.selectedTower != null)
        {
            TowerSelector.selectedTower.DeselectTower();
        }
    }

    
    void Update()
    {
        // Handle cancellation input and reset the flag
        if (cancelPressed)
        {
            cancelPressed = false;
            return;
        }

        // Check if the player is in the process of placing a tower
        if (isPlacingTower && currentTower != null)
        {
            FollowMouse();

            // Handle input to place the tower
            if (playerInputs.TowerPlacement.PlaceTower.triggered)
            {
                // Prevent tower placement if the pointer is over a UI element
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Click was on UI, not placing tower.");
                    return;
                }

                // Double-check that placement is still active before proceeding
                if (!isPlacingTower)
                {
                    Debug.Log("Placement was cancelled, no tower will be placed.");
                    return;
                }

                // Finalize tower placement
                PlaceTower();
            }
        }
    }

    private void FollowMouse()
    {
        // Cast a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Vector3 newPosition;

        // Check if the ray hits any collider
        if (Physics.Raycast(ray, out hit))
        {
            newPosition = hit.point;
        }
        else
        {
            // Define a plane at y = 0 (ground level)
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            // Check if the ray intersects the plane
            if (groundPlane.Raycast(ray, out float enter))
            {
                newPosition = ray.GetPoint(enter); 
            }
            else
            {
                return;
            }
        }
        newPosition.y = 0f; // Ensures the tower stays on the ground
        currentTower.transform.position = newPosition;

        // Stores the calculated position for use in PlaceTower
        towerPlacementPosition = newPosition;
    }

    public void DecreaseTowerPlacementCount(int towerIndex)
    {
        // Validate the tower index to ensure it is within range
        if (towerIndex < 0 || towerIndex >= towerPlacementCounts.Length)
        {
            Debug.LogWarning("Invalid tower index for decrement.");
            return;
        }

        // Decrease the placement count
        if (towerPlacementCounts[towerIndex] > 0)
        {
            towerPlacementCounts[towerIndex]--;
            Debug.Log($"Tower {towerIndex + 1} count decremented. Total placed: {towerPlacementCounts[towerIndex]}");
        }
        else
        {
            Debug.LogWarning($"Tower {towerIndex + 1} placement count is already 0.");
        }
    }

    private void PlaceTower()
    {
        // Ensure there is a tower ready to place
        if (currentTower == null)
        {
            Debug.Log("No tower to place.");
            return;
        }

        // Uses the stored tower position from FollowMouse
        Vector3 finalPosition = towerPlacementPosition;

        Debug.Log($"PlaceTower Position: {finalPosition}");

        // Defined layers for the path and the floor
        int pathLayerMask = LayerMask.GetMask("Path");
        int floorLayerMask = LayerMask.GetMask("Floor");

        // Prevent placing the tower on the path
        if (Physics.CheckSphere(finalPosition, 0.3f, pathLayerMask))
        {
            Debug.Log("Cannot place tower on the path.");
            return;
        }

        // Check for overlapping towers
        Collider[] hitColliders = Physics.OverlapSphere(finalPosition, 0.3f);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Tower") && collider.gameObject != currentTower)
            {
                Debug.Log("Cannot place tower: Another tower is already placed here.");
                return;
            }
        }

        // Ensure placement is on floor layer
        if (Physics.CheckSphere(finalPosition, 0f, floorLayerMask)){
            currentTower.transform.position = finalPosition;
            Debug.Log($"Tower placed at: {finalPosition}");
            currentTower.tag = "Tower";
        }
        else
        {
            Debug.Log("Cannot place tower.");
            return;
        }

        // Deduct currency for the placed tower
        int towerIndex = System.Array.IndexOf(towerPrefabs, selectedTowerPrefab);
        if (towerIndex < 0 || towerIndex >= towerCosts.Length)
        {
            Debug.LogError("Invalid tower index for cost deduction.");
            return;
        }

        int towerCost = towerCosts[towerIndex];

        if (!currencyManager.SpendCurrency(towerCost))
        {
            Debug.Log($"Not enough currency! This tower costs {towerCost} currency");
            return;
        }

        TowerBehaviour towerBehaviour = currentTower.GetComponent<TowerBehaviour>();
        if (towerBehaviour != null)
        {
            towerBehaviour.enabled = true;
            towerBehaviour.SetTowerType(towerIndex); // Set the attack speed (reload time)
            switch (System.Array.IndexOf(towerPrefabs, selectedTowerPrefab))
            {
                case 0: // Tower 1
                    towerBehaviour.towerDamage = 4f;
                    break;
                case 1: // Tower 2
                    towerBehaviour.towerDamage = 6f;
                    break;
                case 2: // Tower 3
                    towerBehaviour.towerDamage = 10f;
                    break;
                default:
                    Debug.LogError("Tower index out of range!");
                    break;
            }
        }

        // Update tower placement count and UI
        if (towerIndex >= 0)
        {
            towerPlacementCounts[towerIndex]++;
            Debug.Log($"Tower {towerIndex + 1} placed. Total placed: {towerPlacementCounts[towerIndex]}");
            UpdateTowerButtonTexts();
        }

        // Mark tower as placed and clean up
        if (currentTower != null)
        {
            TowerSelector towerSelector = currentTower.GetComponent<TowerSelector>();
            if (towerSelector != null)
            {
                towerSelector.MarkAsPlaced();
            }
        }

        currentTower = null;
        isPlacingTower = false;
    }
}
