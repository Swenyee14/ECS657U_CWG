using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelect : MonoBehaviour
{
    // starts a level based on the id given on click
    public void startLevel(int levelID)
    {
        SceneManager.LoadScene(levelID);
    }
}
