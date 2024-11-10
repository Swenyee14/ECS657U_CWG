using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    void Start()
    {
        currencyManager = GameObject.FindGameObjectWithTag("Master").GetComponent<CurrencyManager>();
        // Added listeners to buttons to handle tower placement and cancellation
        addTowerButton.onClick.AddListener(StartTowerPlacement);
        cancelPlacementButton.onClick.AddListener(CancelTowerPlacement);
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

            // Disabled the tower's behavior while placing it
            TowerBehaviour towerBehaviour = currentTower.GetComponent<TowerBehaviour>();
            if (towerBehaviour != null)
            {
                towerBehaviour.enabled = false;
            }

            // Makes the tower follow the mouse
            FollowMouse();
        }
    }

    // Cancels tower placement
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

    // Update is called once per frame
    void Update()
    {
        // If the cancel button was pressed, reset the flag and return
        if (cancelPressed)
        {
            cancelPressed = false;
            return;
        }

        // Checks if the "C" key was pressed and cancels tower placement
        if (Input.GetKeyDown(KeyCode.C))
        {
            CancelTowerPlacement();
        }

        // Checks if the "1" key was pressed to start tower placement
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartTowerPlacement();
        }

        if (isPlacingTower && currentTower != null)
        {
            FollowMouse(); // Makes the tower follow the mouse

            // Checks for left mouse click
            if (Input.GetMouseButtonDown(0))
            {
                // Ensures the click wasn't over a UI element
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

    //Makes the tower follow the mouse position
    private void FollowMouse()
    {
        // Casts a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If the ray hits something, move the tower to the hit point
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 newPosition = hit.point;
            newPosition.y = 0f; // Ensures the tower stays on the ground
            currentTower.transform.position = newPosition; // Moves the tower
        }
    }


    // Places the tower at the clicked location
    private void PlaceTower()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
            if (currentTower.transform.position == hit.point) {
                if(!currencyManager.SpendCurrency(towerCost))
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
