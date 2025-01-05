using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class TowerSelector : MonoBehaviour
{
    public static TowerSelector selectedTower;
    public TowerBehaviour towerBehaviour;
    private CurrencyManager currencyManager;

    public GameObject TowerMenu;
    public Button upgradeButton;
    public Button sellButton;
    public TextMeshProUGUI upgradeLevelText;
    public TextMeshProUGUI towerDamageText;
    public TextMeshProUGUI towerRangeText;
    public TextMeshProUGUI towerFireRateText;
    public TextMeshProUGUI towerNameText;
    private int upgradeCount = 0;
    private int[] upgradeCosts = { 2, 3, 5 };
    public string towerType;
    private bool isPlaced = false;
    private CharacterSelectionManager characterSelectionManager;
    private string GetTowerDisplayName()
    {
        switch (towerType)
        {
            case "Tower1": return "Basic Tower";
            case "Tower2": return "Sniper Tower";
            case "Tower3": return "Bomb Tower";
            default: return "Unknown Tower";
        }
    }

    void Awake()
    {
        towerBehaviour = GetComponent<TowerBehaviour>();
        currencyManager = Object.FindFirstObjectByType<CurrencyManager>();
        characterSelectionManager = Object.FindFirstObjectByType<CharacterSelectionManager>();

        if (UIManager.instance != null)
        {
            TowerMenu = UIManager.instance.towerMenu;
            upgradeButton = UIManager.instance.upgradeButton;
            sellButton = UIManager.instance.sellButton;
            upgradeLevelText = UIManager.instance.upgradeLevelText;
            towerDamageText = UIManager.instance.towerDamageText;
            towerRangeText = UIManager.instance.towerRangeText;
            towerFireRateText = UIManager.instance.towerFireRateText;
            towerNameText = UIManager.instance.towerNameText;
            upgradeButton.onClick.AddListener(UpgradeTower);
            sellButton.onClick.AddListener(SellTower);
        }
    }
    public void MarkAsPlaced()
    {
        isPlaced = true; // Mark the tower as fully placed
    }

    void OnMouseDown()
    {
        if (!isPlaced)
        {
            return;
        }

        if (selectedTower == this)
        {
            DeselectTower();
        }
        else
        {
            if (selectedTower != null)
            {
                selectedTower.DeselectTower();
            }

            SelectTower();
        }
    }

    private void SelectTower()
    {
        selectedTower = this;
        //ShowRangeIndicator();
        ShowTowerMenu();
        if (TowerMenu != null)
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(UpgradeTower);

            sellButton.onClick.RemoveAllListeners();
            sellButton.onClick.AddListener(SellTower);
        }
        UpdateTowerNameText();
        UpdateFireRateText();
        UpdateRangeText();
        UpdateDamageText();
        UpdateUpgradeText();
    }

    public void DeselectTower()
    {
        HideTowerMenu();
        selectedTower = null;
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

    private Dictionary<string, int[]> towerUpgradeCosts = new Dictionary<string, int[]>()
    {
        { "Tower1", new int[] { 5, 8, 12 } }, // Upgrade costs for Tower 1
        { "Tower2", new int[] { 10, 15, 20 } }, // Upgrade costs for Tower 2
        { "Tower3", new int[] { 25, 40, 60 } } // Upgrade costs for Tower 3
    };

    private void UpgradeTower()
    {
        if (upgradeCount >= 3)
        {
            Debug.Log("Maximum upgrades reached.");
            return;
        }

        if (!towerUpgradeCosts.TryGetValue(towerType, out int[] upgradeCosts))
        {
            Debug.LogError("Upgrade costs not defined for this tower type.");
            return;
        }

        int upgradeCost = upgradeCosts[upgradeCount];
        if (CurrencyManager.currency < upgradeCost)
        {
            Debug.Log("Not enough currency to upgrade!");
            return;
        }

        currencyManager.SpendCurrency(upgradeCost);

       
        if (towerType == "Tower1")
        {
            towerBehaviour.towerDamage += 2f;
            towerBehaviour.attackSpeed -= 0.1f;
            towerBehaviour.range += 1f;
        }
        else if (towerType == "Tower2")
        {
            towerBehaviour.towerDamage += 3f;
            towerBehaviour.range += 2.5f;
        }
        else if (towerType == "Tower3")
        {
            towerBehaviour.towerDamage += 5f;
        }
        else
        {
            Debug.LogWarning("Unknown tower type. No damage increase applied.");
        }
        upgradeCount++;
        UpdateTowerNameText();
        UpdateFireRateText();
        UpdateRangeText();
        UpdateDamageText();
        UpdateUpgradeText();
    }

    private void SellTower()
    {
        int sellPrice = 0;

        switch (towerType)
        {
            case "Tower1":
                sellPrice = 1;
                break;
            case "Tower2":
                sellPrice = 2;
                break;
            case "Tower3":
                sellPrice = 3;
                break;
            default:
                Debug.LogWarning("Unknown tower type. Defaulting sell price to 0.");
                break;
        }
        currencyManager.AddCurrency(sellPrice);
        // Use the towerType to determine the correct tower index
        int towerIndex = -1;
        switch (towerType)
        {
            case "Tower1":
                towerIndex = 0;
                break;
            case "Tower2":
                towerIndex = 1;
                break;
            case "Tower3":
                towerIndex = 2;
                break;
            default:
                Debug.LogWarning("Unknown tower type. Could not find matching tower index for decrement.");
                break;
        }

        if (towerIndex >= 0)
        {
            characterSelectionManager.DecreaseTowerPlacementCount(towerIndex);
        }
        else
        {
            Debug.LogWarning("Could not find matching tower index for decrement.");
        }

        if (selectedTower == this)
        {
            HideTowerMenu();
            selectedTower = null; 
        }
        Destroy(gameObject);
    }
    private void UpdateUpgradeText()
    {
        if (upgradeLevelText == null) return;

        if (upgradeCount >= 3)
        {
            upgradeLevelText.text = "[MAX]";
        }
        else
        {
            if (towerUpgradeCosts.TryGetValue(towerType, out int[] upgradeCosts))
            {
                int nextUpgradeCost = upgradeCosts[upgradeCount];
                upgradeLevelText.text = $"Upgrade Cost: {nextUpgradeCost}\n" + $"Current Level: {upgradeCount}";

            }
            else
            {
                Debug.LogWarning("Upgrade costs not defined for this tower type.");
                upgradeLevelText.text = "[Upgrade Cost Unknown]";
            }
        }
    }

    private void UpdateDamageText()
    {
        if (towerDamageText != null)
        {
            towerDamageText.text = $"Damage: {towerBehaviour.towerDamage}";
        }
    }

    private void UpdateRangeText()
    {
        if (towerRangeText != null)
        {
            towerRangeText.text = $"Range: {towerBehaviour.range}";
        }
        
    }

    private void UpdateFireRateText()
    {
        if (towerFireRateText != null)
        {
            towerFireRateText.text = $"Fire rate: {towerBehaviour.attackSpeed}";
        }

    }

    private void UpdateTowerNameText()
    {
        if (towerNameText != null)
        {
            towerNameText.text = GetTowerDisplayName(); // Display the tower type as the name
        }
    }
}
