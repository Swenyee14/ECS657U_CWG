using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showWinUITutorial : MonoBehaviour
{
    public GameObject gameObjectUI;
    public int counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update() //once 15 enemies have been defeated object will be set to active
    {
        if (counter == 20)
            gameObjectUI.SetActive(true);
    }

    public void EnemyCounter()
    {
        counter++;
    }
}
