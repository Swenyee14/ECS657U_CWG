using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class showEndUI : MonoBehaviour

{

    public int counter = 20;
    public GameObject gameObjectUI;
    public Slider playerHealth;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void NeededMethod() //sets inactive object to become active
    {
        counter--;
        playerHealth.value = counter;
        //audioManager.PlaySFX(audioManager.<YOUR SOUND>);
        if (counter == 0)
        {
            if (TowerSelector.selectedTower != null)
            {
                TowerSelector.selectedTower.DeselectTower(); // Hides the menu
            }
            gameObjectUI.SetActive(true);
            Time.timeScale = 0f;
        }

        Debug.Log("This is how many lives you have " + counter);
    }
}
