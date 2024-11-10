using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public TextMeshProUGUI currentCurrency;
    public static int currency;
    public int startCurrency = 2;
    private void Start()
    {
        // Initialize the currency display at the start
        currency = startCurrency;
        UpdateCurrencyUI();
    }

    // Method to add currency (called when an enemy is destroyed)
    public void AddCurrency(int n)
    {
        currency += n;
        UpdateCurrencyUI();
    }

    // Method to subtract currency (called when a tower is placed)
    public bool SpendCurrency(int n)
    {
        if (currency >= n)
        {
            currency -= n;
            UpdateCurrencyUI();
            return true;
        }
        else
        {
            Debug.Log("Not enough currency to place a tower.");
            return false;
        }
    }

    // Updates the currency UI text element
    private void UpdateCurrencyUI()
    {
        currentCurrency.text = currency.ToString();
    }
}
