using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab; 

    void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(SpawnEnemies()); 
    }

    private IEnumerator SpawnEnemies()
    {
        int EnemyCount = 5;

        for (int i = 0; i < EnemyCount; i++)
        {
            Instantiate(EnemyPrefab, new Vector3(-9.7f, 0.46f, 7.991f), Quaternion.identity);
            yield return new WaitForSeconds(3f);
        }

       
    }
}
