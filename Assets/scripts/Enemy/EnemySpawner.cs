using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab; 


    public void SpawnEnemies()
    { 
        Instantiate(EnemyPrefab, new Vector3(-9.7f, 0.46f, 7.991f), Quaternion.identity);
    }
}
