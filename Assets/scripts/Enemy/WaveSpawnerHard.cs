using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawnerHard : MonoBehaviour
{
    public EnemySpawner enemySpawner; // Reference to EnemySpawner component

    public float timeBetweenWaves = 15f;     // 10-second delay between waves
    public float timeBetweenSpawns = 1f;     // 3-second delay between each enemy spawn
    public int enemiesPerWave = 10;           // 5 enemies per wave
    public int totalWaves = 15;               // Total number of waves
    public float initialWaveCountDown;

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
    AudioManager audioManager;

    [System.Obsolete]
    void Start() //method needed to use EnemySpawner.cs code
    {
        if (enemySpawner == null)
        {
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }
        spawnCountDown = timeBetweenSpawns;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        // If we have reached the total wave limit, stop spawning
        if (waveNumber >= totalWaves)
        {
            Debug.Log("Done spawning");
            return;
        }

        if (initialWaveCountDown <= 0)
        {
            if (waveCountDown <= 0)
            {

                //game should end at 15 waves
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

                            //fast enemies spawn starting wave 4
                            if (waveNumber >= 3)
                            {
                                if (FastenemiesSpawnedInWave <= enemiesPerWave)
                                {
                                    enemySpawner.SpawnEnemiesFast();
                                    FastenemiesSpawnedInWave = FastenemiesSpawnedInWave + 2;
                                }

                            }

                            //tank enemies spawn starting wave 4
                            if (waveNumber >= 3)
                            {
                                if (TankenemiesSpawnedInWave <= enemiesPerWave)
                                {
                                    enemySpawner.SpawnEnemiesTank();
                                    TankenemiesSpawnedInWave = TankenemiesSpawnedInWave + 2;
                                }

                            }

                            if (waveNumber >= 5)
                            {
                                if (TankenemiesSpawnedInWave <= enemiesPerWave)
                                {
                                    enemySpawner.SpawnEnemiesTank();
                                    TankenemiesSpawnedInWave = TankenemiesSpawnedInWave + 1;
                                    enemySpawner.SpawnEnemiesFast();
                                    FastenemiesSpawnedInWave = FastenemiesSpawnedInWave + 1;
                                }

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
                        enemiesPerWave = enemiesPerWave + 10;
                        audioManager.PlaySFX(audioManager.waveCompleteSound);
                    }
                    waveUINum.text = Mathf.Floor(waveNumber).ToString(); //AV style 
                }

            }
            else
            {
                waveCountDown -= Time.deltaTime;
            }
        }
        else
        {
            initialWaveCountDown -= Time.deltaTime;
        }

    }
}
