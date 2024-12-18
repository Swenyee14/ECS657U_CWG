using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject EnemyPrefabFast;
    public GameObject EnemyPrefabTank;


    //spawn normal enemies
    public void SpawnEnemies()
    { 
        Instantiate(EnemyPrefab, new Vector3(-9.7f, 0.46f, 7.991f), Quaternion.identity);
    }

    //spawn fast enemies
    public void SpawnEnemiesFast()
    {
        Instantiate(EnemyPrefabFast, new Vector3(-9.7f, 0.46f, 7.991f), Quaternion.identity);
    }

    //spawn tank enemies
    public void SpawnEnemiesTank()
    {
        Instantiate(EnemyPrefabTank, new Vector3(-9.7f, 0.46f, 7.991f), Quaternion.identity);
    }
}
 