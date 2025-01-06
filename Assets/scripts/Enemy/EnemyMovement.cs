using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float health;
    public float maxhealth;
    public float speed;
    public float rotationSpeed = 5f;
    public Transform target;
    public int positionIndex = 0;
    public ParticleSystem deathParticles;
    public int currencyValue = 1;
    showEndUI UIScript;
    showWinUI UICode;
    showWinUIH UIHode;
    showWinUIHHH UIHHHode;

    [SerializeField] HealthBar healthBar;

    void Start()
    {
        if (Positions.positions == null || Positions.positions.Length == 0)
        {
            Debug.LogError("Positions.positions is not set or contains no waypoints.");
            return;
        }
        // reference to the waypoint
        target = Positions.positions[0];
        UIScript = GameObject.FindGameObjectWithTag("Master").GetComponent<showEndUI>();
        UICode = GameObject.FindGameObjectWithTag("Master").GetComponent<showWinUI>();
        UIHode = GameObject.FindGameObjectWithTag("Master").GetComponent<showWinUIH>();
        UIHHHode = GameObject.FindGameObjectWithTag("Master").GetComponent<showWinUIHHH>();
        healthBar = GetComponentInChildren<HealthBar>();
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

    public void Initialize(float healthValue, float speedValue)
    {
        maxhealth = healthValue; // Set max health
        health = healthValue;    // Initialize current health
        speed = speedValue;
        healthBar.UpdateHealthBar(health, maxhealth); // Update health bar UI
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
        if (this == null || gameObject == null)
        {
            Debug.LogWarning("TakeDamage called on a destroyed enemy.");
            return;
        }
        health -= damage;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(health, maxhealth);
        }
        else
        {
            Debug.LogWarning("HealthBar is missing on " + gameObject.name);
        }

        if (health <=0)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            CurrencyManager currencyManager = GameObject.FindGameObjectWithTag("Master").GetComponent<CurrencyManager>();
            if (currencyManager != null)
            {
                currencyManager.AddCurrency(currencyValue);
            }
            target = null;
            Destroy(gameObject);
            ///UIHHHode.DeletingMethodHHH();
            ///UIHode.DeletingMethodH();
            ///UICode.DeletingMethod();
        }
    }
}
