using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    public Button addTowerButton;           
    public Button cancelPlacementButton;    

    public GameObject towerPrefab;         
    private GameObject currentTower;        

    private bool isPlacingTower = false;    

    void Start()
    {
      
        addTowerButton.onClick.AddListener(StartTowerPlacement);
        cancelPlacementButton.onClick.AddListener(CancelTowerPlacement);
    }

    
    private void StartTowerPlacement()
    {
        isPlacingTower = true;

        
        if (currentTower == null)
        {
            currentTower = Instantiate(towerPrefab);
            currentTower.transform.localScale = Vector3.one;
            FollowMouse();
        }
    }

  
    private void CancelTowerPlacement()
    {
        if (currentTower != null)
        {
            Destroy(currentTower);  
            currentTower = null;
            isPlacingTower = false;
            Debug.Log("Tower placement cancelled.");
        }
    }

   
    void Update()
    {
        
        if (isPlacingTower && currentTower != null)
        {
            FollowMouse();

            if (Input.GetMouseButtonDown(0)) 
            {
                PlaceTower();
            }
        }
    }

    
    private void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 newPosition = hit.point;
            newPosition.y = 0f; 
            currentTower.transform.position = newPosition;
        }
    }


    private void PlaceTower()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int pathLayerMask = LayerMask.GetMask("Path");

        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, pathLayerMask))
        {
            Debug.Log($"Cannot place tower on the path: {hit.collider.gameObject.name}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
        }
        else if (Physics.Raycast(ray, out hit))
        {
           
            Collider[] hitColliders = Physics.OverlapSphere(hit.point, 0.5f);  

            foreach (Collider collider in hitColliders)
            {
                if (collider.CompareTag("Tower")) 
                {
                    Debug.Log("Cannot place tower: Another tower is already placed here.");
                    return;  
                }
            }

           
            Debug.Log($"Tower placed at: {hit.point}");
            currentTower.transform.position = hit.point;
            currentTower.tag = "Tower";  
            currentTower = null; 
            isPlacingTower = false; 
        }
        else
        {
            Debug.Log("Raycast didn't hit anything.");
        }
    }

}
