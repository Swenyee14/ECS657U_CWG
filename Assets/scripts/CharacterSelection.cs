using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    public enum CharacterType { Character1, Character2, Character3 }
    public CharacterType selectedCharacter;

    public Button character1Button;
    public Button character2Button;
    public Button character3Button;

    public GameObject towerPrefab; 
    private GameObject currentTower; 

    private bool isPlacingTower = false; 

    void Start()
    {
        character1Button.onClick.AddListener(() => SelectCharacter(CharacterType.Character1));
        character2Button.onClick.AddListener(() => SelectCharacter(CharacterType.Character2));
        character3Button.onClick.AddListener(() => SelectCharacter(CharacterType.Character3));
    }

    private void SelectCharacter(CharacterType characterType)
    {
        
        selectedCharacter = characterType;
        Debug.Log("Selected: " + characterType.ToString());

        if (characterType == CharacterType.Character1)
        {
            
            isPlacingTower = true;

            if (currentTower == null)
            {
                currentTower = Instantiate(towerPrefab); 
                currentTower.transform.localScale = Vector3.one;
                FollowMouse(); 
            }
        }
        else
        {
            isPlacingTower = false;
            if (currentTower != null)
            {
                Destroy(currentTower); 
            }
        }
    }

    void Update()
    {
        // follows mouse
        if (isPlacingTower && currentTower != null)
        {
            FollowMouse();

            // Checks if the user clicks to place the tower
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
        Debug.Log("Tower placed at: " + currentTower.transform.position);
        currentTower = null;
        isPlacingTower = false;
    }
}
