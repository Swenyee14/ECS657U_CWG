using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelect : MonoBehaviour
{
    public void startLevel(int levelID)
    {
        SceneManager.LoadScene(levelID);
    }
}
