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

    // Stores the calculated tower position
    private Vector3 towerPlacementPosition;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
    }

    void Start()
    {
        currencyManager = GameObject.FindGameObjectWithTag("Master").GetComponent<CurrencyManager>();
        addTowerButton.onClick.AddListener(StartTowerPlacement);
        cancelPlacementButton.onClick.AddListener(CancelTowerPlacement);

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
        if (isPlacingTower)
        {
            Debug.Log("Already placing a tower.");
            return;
        }

        isPlacingTower = true;

        if (currentTower == null)
        {
            currentTower = Instantiate(towerPrefab);
            currentTower.transform.localScale = Vector3.one;

            TowerBehaviour towerBehaviour = currentTower.GetComponent<TowerBehaviour>();
            if (towerBehaviour != null)
            {
                towerBehaviour.enabled = false;
            }

            FollowMouse();
        }
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
