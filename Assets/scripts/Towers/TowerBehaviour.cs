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
    public float towerDamage = 4f;
    public AudioClip TowerShot;

    [Header("Unity fields")]
    public string enemyTag = "Enemies";
    private Transform lastEnemyInRange; // Track the last detected enemy

    public GameObject AttackPreFab;
    public Transform attackPoint;
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateEnemy", 0f, 1f); // repeat the update enemy function
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // update enemy focus for the tower
    void UpdateEnemy()
    {
        // Gather all enemies in range
        List<GameObject> allEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag(enemyTag));
        allEnemies.RemoveAll(enemy => Vector3.Distance(transform.position, enemy.transform.position) > range);

        // If there are no enemies in range, clear the target and return
        if (allEnemies.Count == 0)
        {
            enemies = null;
            return;
        }

        // Find the enemy closest to the end of the path
        GameObject firstEnemy = allEnemies.Aggregate((first, next) =>
        {
            EnemyMovement firstMovement = first.GetComponent<EnemyMovement>();
            EnemyMovement nextMovement = next.GetComponent<EnemyMovement>();

            if (firstMovement == null || nextMovement == null)
                return first;

            // Prioritize enemies with higher positionIndex; if equal, use distance to the target
            if (firstMovement.positionIndex != nextMovement.positionIndex)
                return firstMovement.positionIndex > nextMovement.positionIndex ? first : next;

            float firstDistance = Vector3.Distance(first.transform.position, firstMovement.target.position);
            float nextDistance = Vector3.Distance(next.transform.position, nextMovement.target.position);
            return firstDistance < nextDistance ? first : next;
        });

        // Set the target to the first enemy
        enemies = firstEnemy.transform;

        // Check if a new enemy has entered range
        if (enemies != lastEnemyInRange)
        {
            Debug.Log("ENEMY IN RANGE");
            lastEnemyInRange = enemies; // Update the last detected enemy
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Reduce reload timer
        reloadSpeed -= Time.deltaTime;

        // If the reload timer is complete, attempt to shoot
        if (reloadSpeed <= 0f)
        {
            reloadSpeed = 1f / attackSpeed; // Reset reload timer

            if (enemies != null) // Check if an enemy is in range
            {
                Shoot(); // Shoot at the current target
            }
        }

        // Continuously update enemy target
        UpdateEnemy();
    }

    // instantiate the attack prefab and call Travel method on the attack
    void Shoot()
    {
        GameObject attackGameObject = (GameObject)Instantiate(AttackPreFab, attackPoint.position, attackPoint.rotation);
        AttackBehaviour attack = attackGameObject.GetComponent<AttackBehaviour>();

        if (attack != null)
            attack.SetDamage(towerDamage); // set the damage for this tower
            attack.Travel(enemies);
            audioManager.PlaySFX(TowerShot);
    }

    // To visualise the range in testing
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
