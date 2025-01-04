using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject towerMenu;
    public Button upgradeButton;
    public Button sellButton;
    public TextMeshProUGUI upgradeLevelText;
    public TextMeshProUGUI towerDamageText;

    // Singleton instance
    public static UIManager instance;

    void Awake()
    {
        // Ensure only one instance of UIManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
