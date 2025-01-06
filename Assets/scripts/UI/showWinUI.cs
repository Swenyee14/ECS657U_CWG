using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showWinUI : MonoBehaviour
{
    public GameObject gameObjectUI;
    public int counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update() 
    {
        //once 10 enemies have been defeated object will be set to active
        if (counter == 10)
            gameObjectUI.SetActive(true);
    }


    // increase counter
    public void DeletingMethod() 
    {
        counter++;
 

    }
}
