using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public Transform enemies;

    [Header("Tower Stats")]
    public float attackSpeed = 1f;
    public float reloadSpeed = 0f;
    public float range = 3.5f;

    [Header("Unity fields")]
    public string enemyTag = "Enemies";
    private Transform lastEnemyInRange; // Track the last detected enemy

    public GameObject AttackPreFab;
    public Transform attackPoint;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateEnemy", 0f, 1f); // repeat the update enemy function
    }

    // update enemy focus for the tower
    void UpdateEnemy()
    {
        // Get all active enemies within range
        List<GameObject> allEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag(enemyTag));
        allEnemies.RemoveAll(enemy =>
            !enemy.activeInHierarchy ||
            Vector3.Distance(transform.position, enemy.transform.position) > range); // Remove enemies out of range

        if (allEnemies.Count > 0)
        {
            // Find the first enemy based on their positionIndex and proximity to the next waypoint
            GameObject firstEnemy = allEnemies.Aggregate((first, next) =>
            {
                EnemyMovement firstEnemyMovement = first.GetComponent<EnemyMovement>();
                EnemyMovement nextEnemyMovement = next.GetComponent<EnemyMovement>();

                if (firstEnemyMovement == null || nextEnemyMovement == null)
                    return first; // Fallback if components are missing

                // Compare by positionIndex; if equal, target the closest to their next waypoint
                if (firstEnemyMovement.positionIndex != nextEnemyMovement.positionIndex)
                {
                    return firstEnemyMovement.positionIndex > nextEnemyMovement.positionIndex ? first : next;
                }

                // If positionIndex is the same, choose based on distance to the current target waypoint
                float firstDistance = Vector3.Distance(first.transform.position, firstEnemyMovement.target.position);
                float nextDistance = Vector3.Distance(next.transform.position, nextEnemyMovement.target.position);

                return firstDistance < nextDistance ? first : next;
            });

            enemies = firstEnemy.transform;

            // Check if a new enemy has been targeted
            if (enemies != lastEnemyInRange)
            {
                Debug.Log("FIRST ENEMY TARGETED");
                lastEnemyInRange = enemies; // Update the last detected enemy
            }
        }
        else
        {
            // No enemies in range
            enemies = null;
            lastEnemyInRange = null; // Reset last enemy
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (enemies == null)
            return;

        // shoot in intervals
        if (reloadSpeed <= 0f)
        {
            Shoot();
            reloadSpeed = 1f / attackSpeed;
        }
        reloadSpeed = reloadSpeed - Time.deltaTime;
    }

    // instantiate the attack prefab and call Travel method on the attack
    void Shoot()
    {
        GameObject attackGameObject = (GameObject)Instantiate(AttackPreFab, attackPoint.position, attackPoint.rotation);
        AttackBehaviour attack = attackGameObject.GetComponent<AttackBehaviour>();

        if (attack != null)
            attack.Travel(enemies);
    }

    // To visualise the range in testing
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
