using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showWinUIHHH : MonoBehaviour
{
    public GameObject gameObjectUI;
    public int counterHHH;

    // Start is called before the first frame update
    void Start()
    {
        counterHHH = 0;
    }

    // Update is called once per frame
    void Update() //once 15 enemies have been defeated object will be set to active
    {
        if (counterHHH == 2050)
            gameObjectUI.SetActive(true);
    }


    /// <summary>
    /// Needed
    /// </summary>
    public void DeletingMethodHHH() //sets inactive object to become active
    {
        counterHHH++;
    }
}
