using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    public enum CharacterType { Character1, Character2, Character3 }
    public CharacterType selectedCharacter;

    public Button character1Button;
    public Button character2Button;
    public Button character3Button;

    void Start()
    {
        // Assign click listeners to the buttons
        character1Button.onClick.AddListener(() => SelectCharacter(CharacterType.Character1));
        character2Button.onClick.AddListener(() => SelectCharacter(CharacterType.Character2));
        character3Button.onClick.AddListener(() => SelectCharacter(CharacterType.Character3));
    }

    private void SelectCharacter(CharacterType characterType)
    {
        // Update the selected character
        selectedCharacter = characterType;
        Debug.Log("Selected: " + characterType.ToString());
    }
}
