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
    public float initialWaveCountDown;
    public GameObject gameObjectUI;
    float CTime = 0f;                        // current time
    float STime = 760f;                      // starting time


    private float waveCountDown = 2f;
    private float spawnCountDown;
    public int waveNumber = 0;
    private int enemiesSpawnedInWave = 0;
    private int FastenemiesSpawnedInWave = 0;
    private int TankenemiesSpawnedInWave = 0;

    public TextMeshProUGUI waveUINum;
    AudioManager audioManager;

    [System.Obsolete]
    void Start() //method needed to use EnemySpawner.cs code
    {
        CTime = STime;

        if (enemySpawner == null)
        {
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }
        spawnCountDown = timeBetweenSpawns;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        

        CTime -= 1 * Time.deltaTime;

        if (CTime <= 0)
        {
            gameObjectUI.SetActive(true);

        }

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
                                    FastenemiesSpawnedInWave += 5;
                                }

                            }

                            //tank enemies spawn starting wave 8
                            if (waveNumber >= 7)
                            {
                                if (TankenemiesSpawnedInWave <= enemiesPerWave)
                                {
                                    enemySpawner.SpawnEnemiesTank();
                                    TankenemiesSpawnedInWave += 5;
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
                        enemiesPerWave += 5;
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
