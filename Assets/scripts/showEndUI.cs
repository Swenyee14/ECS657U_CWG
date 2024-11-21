using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showEndUI : MonoBehaviour

{

    private int counter = 20;

    public GameObject gameObjectUI;

    public void NeededMethod() //sets inactive object to become active
    {

        if (counter == 0)
        {
            gameObjectUI.SetActive(true);
        }
        else
        {
            counter--;
        }

        Debug.Log("This is how many lives you have" + counter);
        
    }
}
