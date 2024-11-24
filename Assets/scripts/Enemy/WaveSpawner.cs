using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public EnemySpawner enemySpawner; // Reference to EnemySpawner component

    public float timeBetweenWaves = 10f;     // 10-second delay between waves
    public float timeBetweenSpawns = 1f;     // 3-second delay between each enemy spawn
    public int enemiesPerWave = 5;           // 5 enemies per wave
    public int totalWaves = 15;               // Total number of waves

    /// <summary>
    /// These numbers above will eventually be changed into global variables that have numbers linked thoughout the gamemaster :D
    /// </summary>

    private float waveCountDown = 2f;
    private float spawnCountDown;
    private int waveNumber = 0;
    private int enemiesSpawnedInWave = 0;
    private int FastenemiesSpawnedInWave = 0;
    private int TankenemiesSpawnedInWave = 0;

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
            Debug.Log("Done spawning");
            return;
        }

        if (waveCountDown <= 0)
        {

            if (waveNumber <= 15)
            {
                // Spawn enemies with a delay within a single wave
                if (enemiesSpawnedInWave < enemiesPerWave)
                {
                    if (spawnCountDown <= 0f)
                    {
                        enemySpawner.SpawnEnemies();
                        enemiesSpawnedInWave++;
                        spawnCountDown = timeBetweenSpawns;

                        if (waveNumber >= 3)
                        {
                            enemySpawner.SpawnEnemiesFast();
                            FastenemiesSpawnedInWave = FastenemiesSpawnedInWave + 5;

                        }

                        if (waveNumber >= 7)
                        {
                            enemySpawner.SpawnEnemiesTank();
                            TankenemiesSpawnedInWave = TankenemiesSpawnedInWave + 5;

                        }


                    }
                    spawnCountDown -= Time.deltaTime;
                }
                else
                {
                    // Reset for the next wave
                    waveNumber++;
                    enemiesSpawnedInWave = 0;
                    FastenemiesSpawnedInWave = 0;
                    TankenemiesSpawnedInWave = 0;
                    waveCountDown = timeBetweenWaves;
                    enemiesPerWave = enemiesPerWave + 5;
                }
                waveUINum.text = Mathf.Floor(waveNumber).ToString(); //AV style 
            }
            
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }

        //waveUINum.text = Mathf.Floor(waveNumber).ToString();
    }
}
