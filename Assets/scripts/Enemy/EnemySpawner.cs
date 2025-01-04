using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject EnemyPrefab;
    public GameObject EnemyPrefabFast;
    public GameObject EnemyPrefabTank;

    public GameObject position;

    [Header("Enemy Stats")]
    public EnemyStats enemyStats;

    //spawn normal enemies
    public void SpawnEnemies()
    {
        GameObject enemy = Instantiate(EnemyPrefab, position.transform.position, Quaternion.identity);
        Debug.Log("Enemy instantiated: " + enemy.name);

        StartCoroutine(InitializeEnemyAfterDelay(enemy));
    }

    //spawn fast enemies
    public void SpawnEnemiesFast()
    {
        GameObject enemy = Instantiate(EnemyPrefabFast, position.transform.position, Quaternion.identity);
        StartCoroutine(InitializeEnemyAfterDelay(enemy));

    }

    //spawn tank enemies
    public void SpawnEnemiesTank()
    {
        GameObject enemy = Instantiate(EnemyPrefabTank, position.transform.position, Quaternion.identity);
        StartCoroutine(InitializeEnemyAfterDelay(enemy));
    }

    private IEnumerator InitializeEnemyAfterDelay(GameObject enemy)
    {
        // Wait for one frame to ensure that all components are fully initialized
        yield return null;

        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            Debug.Log("Initializing enemy with health: " + enemyStats.standardHealth);
            enemyMovement.Initialize(enemyStats.standardHealth);
        }
        else
        {
            Debug.LogError("EnemyMovement component not found!");
        }
    }
}