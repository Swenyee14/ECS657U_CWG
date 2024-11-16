using UnityEngine;
using UnityEngine.UI;
public class TowerSelector : MonoBehaviour
{
    public static TowerSelector selectedTower;
    public GameObject rangeIndicator;
    public TowerBehaviour towerBehaviour;
    private CurrencyManager currencyManager;
    // Add these fields for TowerMenu and buttons
    public GameObject TowerMenu;
    public Button upgradeButton;
    public Button sellButton;

    private int upgradeCount = 0;
    private int[] upgradeCosts = { 2, 3, 5 };
    void Awake()
    {
        towerBehaviour = GetComponent<TowerBehaviour>();
        currencyManager = Object.FindFirstObjectByType<CurrencyManager>();

        // Get TowerMenu and buttons from UIManager
        if (UIManager.instance != null)
        {
            TowerMenu = UIManager.instance.towerMenu;
            upgradeButton = UIManager.instance.upgradeButton;
            sellButton = UIManager.instance.sellButton;

            // Assign button actions
            upgradeButton.onClick.AddListener(UpgradeTower);
            sellButton.onClick.AddListener(SellTower);
        }
    }

    void OnMouseDown()
    {
        if (selectedTower == this)
        {
            DeselectTower();
        }
        else
        {
            // Deselect the previous tower
            if (selectedTower != null)
            {
                selectedTower.DeselectTower();
            }

            // Set this tower as the selected tower
            SelectTower();
        }
    }

    private void SelectTower()
    {
        selectedTower = this;
        ShowRangeIndicator();
        ShowTowerMenu();
    }

    private void DeselectTower()
    {
        Destroy(rangeIndicator);
        HideTowerMenu();
        selectedTower = null;
    }

    private void ShowRangeIndicator()
    {
        if (towerBehaviour == null) return;

        // Create a sphere object to represent the range
        rangeIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rangeIndicator.transform.position = transform.position;
        rangeIndicator.transform.localScale = new Vector3(towerBehaviour.range * 2, 0.1f, towerBehaviour.range * 2);

        // Set the sphere to be transparent
        Material transparentMaterial = new Material(Shader.Find("Transparent/Diffuse"));
        transparentMaterial.color = new Color(0, 0.5f, 1f, 0.4f); 
        rangeIndicator.GetComponent<Renderer>().material = transparentMaterial;

        // Disable collider on the range indicator to avoid interference with clicks
        Destroy(rangeIndicator.GetComponent<Collider>());

        // Make the range indicator a child of this tower for easy cleanup
        rangeIndicator.transform.SetParent(transform);
    }

    private void HideTowerMenu()
    {
        if (TowerMenu != null)
        {
            TowerMenu.SetActive(false);
        }
    }

    private void ShowTowerMenu()
    {
        if (TowerMenu == null) return;
        TowerMenu.SetActive(true);
    }

    private void UpgradeTower()
    {
        // Check if the upgrade count is less than 3
        if (upgradeCount >= 3)
        {
            Debug.Log("Maximum upgrades reached.");
            return; 
        }

        // Check if the player has enough currency to upgrade
        int upgradeCost = upgradeCosts[upgradeCount];
        if (CurrencyManager.currency < upgradeCost)
        {
            Debug.Log("Not enough currency to upgrade!");
            return;
        }

        // Deduct the currency for the upgrade
        currencyManager.SpendCurrency(upgradeCost);

        // Apply the upgrade
        towerBehaviour.attackSpeed += 0.5f;
        towerBehaviour.range += 1f;

        // Update the range indicator immediately
        UpdateRangeIndicator();

        // Increment the upgrade count
        upgradeCount++;
    }

    private void SellTower()
    {
        currencyManager.AddCurrency(1);
        Destroy(gameObject);
    }

    // Update the range indicator based on the current tower range
    private void UpdateRangeIndicator()
    {
        if (rangeIndicator != null)
        {
            rangeIndicator.transform.localScale = new Vector3(towerBehaviour.range * 2, 0.1f, towerBehaviour.range * 2);
        }
    }
}