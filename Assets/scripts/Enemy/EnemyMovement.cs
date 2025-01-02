using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float health = 5f;
    public float maxhealth = 5f;
    public float speed = 6f;
    public float rotationSpeed = 5f;
    public Transform target;
    public int positionIndex = 0;
    showEndUI UIScript;
    

    [SerializeField] HealthBar healthBar;

    void Start()
    {
        // reference to the waypoint
        target = Positions.positions[0];
        UIScript = GameObject.FindGameObjectWithTag("Master").GetComponent<showEndUI>();
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealthBar(health, maxhealth); // Use this line whenever a takedamage function is added
    }

    void Update()
    {
        // travel in the direction of the waypoints
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        Rotate();

        // get the next positions location once an enemy is close to the current one
        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            GetNextPosition();
        }
    }

    void GetNextPosition()
    {
        // Destroy the enemy and end the game if it reaches the end
        if (positionIndex == Positions.positions.Length - 1)
        {
            Destroy(gameObject);
            UIScript.NeededMethod();
            return;
        }

        // get next position
        positionIndex++;
        target = Positions.positions[positionIndex];
    }

    private void Rotate()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <=0)
        {
            Destroy(gameObject);
        }
    }
}
