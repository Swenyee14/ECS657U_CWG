using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
    private int towerCost = 1;
    private CurrencyManager currencyManager;

    private PlayerInputs playerInputs;

    // Stores the calculated tower position
    private Vector3 towerPlacementPosition;

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
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    private void SelectTower(int towerIndex)
    {
        if (towerIndex < 0 || towerIndex >= towerPrefabs.Length)
        {
            Debug.LogError("Invalid tower index selected.");
            return;
        }

        if (isPlacingTower)
        {
            CancelTowerPlacement();
        }

        selectedTowerPrefab = towerPrefabs[towerIndex];
        StartTowerPlacement();
        Debug.Log($"Tower {towerIndex + 1} selected: {selectedTowerPrefab.name}");
    }

    private void StartTowerPlacement()
    {
        if (isPlacingTower)
        {
            Debug.Log("Already placing a tower.");
            return;
        }

        if (selectedTowerPrefab == null)
        {
            Debug.LogError("No tower prefab selected!");
            return;
        }

        isPlacingTower = true;

        currentTower = Instantiate(selectedTowerPrefab);
        currentTower.transform.localScale = Vector3.one;

        TowerBehaviour towerBehaviour = currentTower.GetComponent<TowerBehaviour>();
        if (towerBehaviour != null)
        {
            towerBehaviour.enabled = false;
        }

        FollowMouse();
    }

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

    void Update()
    {
        if (cancelPressed)
        {
            cancelPressed = false;
            return;
        }

        if (isPlacingTower && currentTower != null)
        {
            FollowMouse();

            if (playerInputs.TowerPlacement.PlaceTower.triggered)
            {
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

                PlaceTower();
            }
        }
    }

    private void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 newPosition = hit.point;
            newPosition.y = 0f; // Ensures the tower stays on the ground
            currentTower.transform.position = newPosition;

            // Stores the calculated position for use in PlaceTower
            towerPlacementPosition = newPosition;

            Debug.Log($"FollowMouse Position: {towerPlacementPosition}");
        }
    }

    private void PlaceTower()
    {
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

        currentTower.transform.position = finalPosition;
        Debug.Log($"Tower placed at: {finalPosition}");
        currentTower.tag = "Tower";

        if (!currencyManager.SpendCurrency(towerCost))
        {
            Debug.Log("Not enough currency!");
            return;
        }

        TowerBehaviour towerBehaviour = currentTower.GetComponent<TowerBehaviour>();
        if (towerBehaviour != null)
        {
            towerBehaviour.enabled = true;
        }

        currentTower = null;
        isPlacingTower = false;
    }
}
