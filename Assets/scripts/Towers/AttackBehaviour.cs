using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    showWinUI UIScript;
    private CurrencyManager currencyManager; 
    private Transform enemies;
    public float speed = 20f;
    public float damage;
    public float AoeRadius = 0f;
    public ParticleSystem trail;
    public void Travel(Transform _enemies)
    {
        enemies = _enemies;
    }
    public void SetDamage(float damageValue)
    {
        damage = damageValue; // Set damage based on tower type
    }

    private void Start() //allows access to other methods in different scripts
    {
        UIScript = GameObject.FindGameObjectWithTag("Master").GetComponent<showWinUI>();
        currencyManager = GameObject.FindGameObjectWithTag("Master").GetComponent<CurrencyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Instantiate(trail, transform.position, Quaternion.identity);
        // destroy attack if enemy is out of range 
        if (enemies == null)
        {
            Destroy(gameObject);
            return;
        }

        // if the distance between the tower and enemy is less than or equal to the distance the attack can travel in a frame
        // then handle collision with enemy
        Vector3 direction = enemies.position - transform.position;
        float distancePerFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distancePerFrame)
        {
            CollisionWithEnemy();
            return;
        }

        transform.Translate(direction.normalized * distancePerFrame, Space.World);

    }

    // handle collision with enemy by destroying both attack and enemy game object
    void CollisionWithEnemy()
    {
        if (AoeRadius > 0f)
        {
            // Find all colliders within the AOE radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, AoeRadius);

            foreach (Collider collider in colliders)
            {
                // Get the EnemyMovement script on each collider
                EnemyMovement enemyScript = collider.GetComponent<EnemyMovement>();

                // Apply damage if the collider has an EnemyMovement script
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(damage);
                }
            }
        }
        else
        {
            // Single target logic
            EnemyMovement enemyScript = enemies.GetComponent<EnemyMovement>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
        }

        //UIScript.EnemyCounter(); //this line specifically calls a method in another script
        Destroy(gameObject);
    }
}
