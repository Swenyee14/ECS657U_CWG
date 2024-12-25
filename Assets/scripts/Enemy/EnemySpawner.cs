using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject EnemyPrefabFast;
    public GameObject EnemyPrefabTank;
    public GameObject position;


    //spawn normal enemies
    public void SpawnEnemies()
    { 
        Instantiate(EnemyPrefab, position.transform.position, Quaternion.identity);
    }

    //spawn fast enemies
    public void SpawnEnemiesFast()
    {
        Instantiate(EnemyPrefabFast, position.transform.position, Quaternion.identity);
    }

    //spawn tank enemies
    public void SpawnEnemiesTank()
    {
        Instantiate(EnemyPrefabTank, position.transform.position, Quaternion.identity);
    }
}
 