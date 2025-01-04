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
        StartCoroutine(InitializeStandardEnemyAfterDelay(enemy));
    }

    //spawn fast enemies
    public void SpawnEnemiesFast()
    {
        GameObject enemy = Instantiate(EnemyPrefabFast, position.transform.position, Quaternion.identity);
        StartCoroutine(InitializeFastEnemyAfterDelay(enemy, 3));

    }

    //spawn tank enemies
    public void SpawnEnemiesTank()
    {
        GameObject enemy = Instantiate(EnemyPrefabTank, position.transform.position, Quaternion.identity);
        StartCoroutine(InitializeTankEnemyAfterDelay(enemy, 5));
    }

    private IEnumerator InitializeStandardEnemyAfterDelay(GameObject enemy, int currencyValue)
    {
        // Wait for one frame to ensure that all components are fully initialized
        yield return null;

        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            Debug.Log("Initializing enemy with health: " + enemyStats.standardHealth);
            enemyMovement.Initialize(enemyStats.standardHealth);
            enemyMovement.currencyValue = currencyValue;
        }
        else
        {
            Debug.LogError("EnemyMovement component not found!");
        }
    }
    private IEnumerator InitializeFastEnemyAfterDelay(GameObject enemy, int currencyValue)
    {
        // Wait for one frame to ensure that all components are fully initialized
        yield return null;

        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            Debug.Log("Initializing enemy with health: " + enemyStats.fastHealth);
            enemyMovement.Initialize(enemyStats.fastHealth);
            enemyMovement.currencyValue = currencyValue;
        }
        else
        {
            Debug.LogError("EnemyMovement component not found!");
        }
    }
    private IEnumerator InitializeTankEnemyAfterDelay(GameObject enemy, int currencyValue)
    {
        // Wait for one frame to ensure that all components are fully initialized
        yield return null;

        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            Debug.Log("Initializing enemy with health: " + enemyStats.tankHealth);
            enemyMovement.Initialize(enemyStats.tankHealth);
            enemyMovement.currencyValue = currencyValue;
        }
        else
        {
            Debug.LogError("EnemyMovement component not found!");
        }
    }
}