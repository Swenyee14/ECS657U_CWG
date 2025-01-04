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
    public float reloadTime = 0f;
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
        // Always decrease reload timer, regardless of enemy presence
        reloadTime -= Time.deltaTime;

        // If the reload timer has finished, shoot
        if (reloadTime <= 0f)
        {
            if (enemies != null)
            {
                Shoot(); // Shoot at the current enemy target
                reloadTime = attackSpeed; // Reset the reload timer
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

    public void SetTowerType(int towerType)
    {
        switch (towerType)
        {
            case 0: // Tower 1
                attackSpeed = 1.25f;
                break;
            case 1: // Tower 2
                attackSpeed = 2f;
                break;
            case 2: // Tower 3
                attackSpeed = 5f;
                break;
            default:
                Debug.LogError("Unknown tower type.");
                break;
        }

        reloadTime = 0f; // Allow the first shot to fire immediately after placement
    }

    // To visualise the range in testing
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
