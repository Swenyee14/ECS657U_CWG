using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        // Check if a saved volume setting exists; if not, initialize with the default value
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVolume();
        }
    }

    // gets volume value and sets the master volume
    public void SetMasterVolume()
    {
        float volume = volumeSlider.value;  
        mixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume); //saves the value
    }

    // Loads and applies saved to the slider and mixer
    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        SetMasterVolume();
    }
}
