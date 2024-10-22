using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    showWinUI UIScript;
    private Transform enemies;
    public float speed = 20f;
    public void Travel(Transform _enemies)
    {
        enemies = _enemies;
    }

    private void Start() //allows access to other methods in different scripts
    {
        UIScript = GameObject.FindGameObjectWithTag("Master").GetComponent<showWinUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = enemies.position - transform.position;
        float distancePerFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distancePerFrame)
        {
            CollisionWithEnemy();
            return;
        }

        transform.Translate(direction.normalized * distancePerFrame, Space.World);

    }

    void CollisionWithEnemy()
    {
        UIScript.EnemyCounter(); //this line specifically calls a method in another script
        Destroy(enemies.gameObject);
        Destroy(gameObject);
    }
}
