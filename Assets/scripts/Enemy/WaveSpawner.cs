using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public EnemySpawner enemySpawner; // Reference to EnemySpawner component

    public float timeBetweenWaves = 10f;     // 10-second delay between waves
    public float timeBetweenSpawns = 3f;     // 3-second delay between each enemy spawn
    public int enemiesPerWave = 5;           // 5 enemies per wave
    public int totalWaves = 3;               // Total number of waves

    /// <summary>
    /// These numbers above will eventually be changed into global variables that have numbers linked thoughout the gamemaster :D
    /// </summary>

    private float waveCountDown = 2f;
    private float spawnCountDown;
    private int waveNumber = 0;
    private int enemiesSpawnedInWave = 0;

    public TextMeshProUGUI waveUINum;

    [System.Obsolete]
    void Start() //method needed to use EnemySpawner.cs code
    {
        if (enemySpawner == null)
        {
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }
        spawnCountDown = timeBetweenSpawns;
    }

    void Update()
    {
        // If we have reached the total wave limit, stop spawning
        if (waveNumber >= totalWaves)
        {
            return;
        }

        if (waveCountDown <= 0)
        {
            // Spawn enemies with a delay within a single wave
            if (enemiesSpawnedInWave < enemiesPerWave)
            {
                if (spawnCountDown <= 0f)
                {
                    enemySpawner.SpawnEnemies();
                    enemiesSpawnedInWave++;
                    spawnCountDown = timeBetweenSpawns;
                }
                spawnCountDown -= Time.deltaTime;
            }
            else
            {
                // Reset for the next wave
                waveNumber++;
                enemiesSpawnedInWave = 0;
                waveCountDown = timeBetweenWaves;
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }

        waveUINum.text = Mathf.Floor(waveNumber).ToString();
    }
}
