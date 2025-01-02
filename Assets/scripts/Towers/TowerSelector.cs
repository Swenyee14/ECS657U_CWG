using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelector : MonoBehaviour
{
    public static TowerSelector selectedTower;
    //public GameObject rangeIndicator;
    public TowerBehaviour towerBehaviour;
    private CurrencyManager currencyManager;

    public GameObject TowerMenu;
    public Button upgradeButton;
    public Button sellButton;
    public TextMeshProUGUI upgradeLevelText;

    private int upgradeCount = 0;
    private int[] upgradeCosts = { 2, 3, 5 };
    public string towerType;

    // Reference to the range indicator sprite prefab (assign in the Inspector)
    //public GameObject rangeIndicatorPrefab;

    void Awake()
    {
        towerBehaviour = GetComponent<TowerBehaviour>();
        currencyManager = Object.FindFirstObjectByType<CurrencyManager>();

        if (UIManager.instance != null)
        {
            TowerMenu = UIManager.instance.towerMenu;
            upgradeButton = UIManager.instance.upgradeButton;
            sellButton = UIManager.instance.sellButton;
            upgradeLevelText = UIManager.instance.upgradeLevelText;

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
        UpdateUpgradeText();
    }

    private void DeselectTower()
    {
        //if (rangeIndicator != null)
        //{
            //Destroy(rangeIndicator);
        //}
        HideTowerMenu();
        selectedTower = null;
    }

    /*
    private void ShowRangeIndicator()
    {
        if (towerBehaviour == null || rangeIndicatorPrefab == null) return;

        // Instantiate the range indicator prefab
        rangeIndicator = Instantiate(rangeIndicatorPrefab, transform.position, Quaternion.identity);

        // Adjust scale based on tower range
        float rangeDiameter = towerBehaviour.range * 2;
        rangeIndicator.transform.localScale = new Vector3(rangeDiameter, rangeDiameter, 1f);

        // Set the range indicator to be a child of the tower
        rangeIndicator.transform.SetParent(transform);
    }
    */

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
        if (upgradeCount >= 3)
        {
            Debug.Log("Maximum upgrades reached.");
            return;
        }

        int upgradeCost = upgradeCosts[upgradeCount];
        if (CurrencyManager.currency < upgradeCost)
        {
            Debug.Log("Not enough currency to upgrade!");
            return;
        }

        currencyManager.SpendCurrency(upgradeCost);

        towerBehaviour.attackSpeed += 0.5f;
        towerBehaviour.range += 1f;
        if (towerType == "Tower1")
        {
            towerBehaviour.towerDamage += 2f;
        }
        else if (towerType == "Tower2")
        {
            towerBehaviour.towerDamage += 3f;
        }
        else if (towerType == "Tower3")
        {
            towerBehaviour.towerDamage += 5f;
        }
        else
        {
            Debug.LogWarning("Unknown tower type. No damage increase applied.");
        }

        //UpdateRangeIndicator();
        upgradeCount++;
        UpdateUpgradeText();
    }

    private void SellTower()
    {
        currencyManager.AddCurrency(1);
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
            upgradeLevelText.text = $"[{upgradeCount}]";
        }
    }

    /*private void UpdateRangeIndicator()
    {
        if (rangeIndicator != null)
        {
            float rangeDiameter = towerBehaviour.range * 2;
            rangeIndicator.transform.localScale = new Vector3(rangeDiameter, rangeDiameter, 1f);
        }
    }*/
}
