using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showEndUI : MonoBehaviour

{

    private int counter = 1;

    public GameObject gameObjectUI;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void NeededMethod() //sets inactive object to become active
    {

        if (counter == 0)
        {
            gameObjectUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            counter--;
            //audioManager.PlaySFX(audioManager.<YOUR SOUND>);
        }

        Debug.Log("This is how many lives you have " + counter);
        
    }
}
