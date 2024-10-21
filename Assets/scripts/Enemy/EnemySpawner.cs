using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab; 

    void Start()
    {
        Time.timeScale = 1f;
        // call enemy spawning function
        StartCoroutine(SpawnEnemies()); 
    }

    private IEnumerator SpawnEnemies()
    {
        int EnemyCount = 15;

        // instantiate until the enemies is less than the enemy count
        for (int i = 0; i < EnemyCount; i++)
        {
            // spawn enemies every 3 seconds
            yield return new WaitForSeconds(3f);
            Instantiate(EnemyPrefab, new Vector3(-9.7f, 0.46f, 7.991f), Quaternion.identity);
        }

       
    }
}
