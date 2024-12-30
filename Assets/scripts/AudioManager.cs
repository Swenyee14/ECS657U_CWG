using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] AudioSource SFXSource;

    [Header("Clips")]
    public AudioClip tower1Shot;
    public AudioClip tower2Shot;
    public AudioClip tower3Shot;
    public AudioClip waveCompleteSound;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
