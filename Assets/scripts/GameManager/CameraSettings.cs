using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSettings : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider speedSlider;

    private void Start()
    {
        // loads settings if they are saved
        if (PlayerPrefs.HasKey("sensitivity") && PlayerPrefs.HasKey("speed"))
        {
            LoadSettings();
        }
        else
        {
            SetSensitivity();
            SetSpeed();
        }
    }

    // gets sensitivity value and saves it
    public void SetSensitivity()
    {
        int sens = Mathf.RoundToInt(sensitivitySlider.value);
        PlayerPrefs.SetInt("sensitivity", sens);
        PlayerPrefs.Save();
    }

    //gets and saves speed value
    public void SetSpeed() 
    {
        int speed = Mathf.RoundToInt(speedSlider.value);
        PlayerPrefs.SetInt("speed", speed);
        PlayerPrefs.Save();
    }

    // loads saved speed and sensitivity
    private void LoadSettings()
    {
        sensitivitySlider.value = PlayerPrefs.GetInt("sensitivity");
        SetSensitivity();
        speedSlider.value = PlayerPrefs.GetInt("speed");
        SetSpeed();
    }
}
