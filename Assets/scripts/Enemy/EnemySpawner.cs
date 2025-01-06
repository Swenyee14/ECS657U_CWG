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
        StartCoroutine(InitializeStandardEnemyAfterDelay(enemy, 1));
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

        //initialise enemies with set health from the scriptable objectfor standards
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.Initialize(enemyStats.standardHealth, enemyStats.standardSpeed);
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

        //initialise enemies with set health from the scriptable object for fasts
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.Initialize(enemyStats.fastHealth, enemyStats.fastSpeed);
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

        //initialise enemies with set health from the scriptable object for tanks
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.Initialize(enemyStats.tankHealth, enemyStats.tankSpeed);
            enemyMovement.currencyValue = currencyValue;
        }
        else
        {
            Debug.LogError("EnemyMovement component not found!");
        }
    }
}