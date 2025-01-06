using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showWinUIH : MonoBehaviour
{
    public GameObject gameObjectUI;
    public int counterH;

    // Start is called before the first frame update
    void Start()
    {
        counterH = 0;
    }

    // Update is called once per frame
    void Update() //once 15 enemies have been defeated object will be set to active
    {
        if (counterH == 840)
            gameObjectUI.SetActive(true);
    }


    /// <summary>
    /// Needed
    /// </summary>
    public void DeletingMethodH() //sets inactive object to become active
    {
        counterH++;


    }
}
