using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    showWinUI UIScript;
    private CurrencyManager currencyManager; 
    private Transform enemies;
    public float speed = 20f;
    private float damage;
    public void Travel(Transform _enemies, float _damage)
    {
        enemies = _enemies;
        damage = _damage;
    }

    private void Start() //allows access to other methods in different scripts
    {
        UIScript = GameObject.FindGameObjectWithTag("Master").GetComponent<showWinUI>();
        currencyManager = GameObject.FindGameObjectWithTag("Master").GetComponent<CurrencyManager>();
    }

    // Update is called once per frame
    void Update()
    {
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
        UIScript.EnemyCounter(); //this line specifically calls a method in another script
        EnemyMovement enemy = enemies.GetComponent<EnemyMovement>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Damage the enemy
        }

        Destroy(gameObject);
    }
}
