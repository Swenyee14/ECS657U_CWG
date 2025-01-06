using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pause;
    
    //pauses game
    public void Pause()
    {
        pause.SetActive(true);
        Time.timeScale = 0f;
    }

    // resumes game
    public void Resume()
    {
        pause.SetActive(false);
        Time.timeScale = 1f;
    }

    // restarts game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    // returns to home screen
    public void Home()
    {
        SceneManager.LoadScene("Title");
        Time.timeScale = 1f;
    }
}
