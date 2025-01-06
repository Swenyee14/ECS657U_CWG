using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject gameObjectUI;
    float CTime = 0f; //current time
    float STime = 52f; //starting time

    // Start is called before the first frame update
    void Start()
    {
        CTime = STime;
    }

    // Update is called once per frame
    void Update()
    {
        CTime -= 1 * Time.deltaTime;

        // runs if the time for the scene is over
        if (CTime <= 0)
        {
            gameObjectUI.SetActive(true);
        }
    }
}
