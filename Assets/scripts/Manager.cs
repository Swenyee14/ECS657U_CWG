using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public GameObject gameOverUI;
    public static bool gameEnded = false;

    // Update is called once per frame
    void Update()
    {
        if (gameEnded)
            return;
    }

    void endOfGame()
    {
        gameEnded = true;
        gameOverUI.SetActive(true);
    }
}
