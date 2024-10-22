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
    private float reloadSpeed = 0f;
    public float range = 5f;

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
        // add and store all enemies with enemy tag in range and remove them once they leave the range
        List<GameObject> allEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag(enemyTag));
        allEnemies.RemoveAll(enemy => Vector3.Distance(transform.position, enemy.transform.position) > range); // Remove enemies out of range

        //change the target enemy for the tower
        if (allEnemies.Count > 0)
        {
            GameObject nearestEnemy = allEnemies.Aggregate((closest, next) =>
                Vector3.Distance(transform.position, next.transform.position) <
                Vector3.Distance(transform.position, closest.transform.position) ? next : closest);

            enemies = nearestEnemy.transform;
            // Check if a new enemy has entered range
            if (enemies != lastEnemyInRange)
            {
                Debug.Log("ENEMY IN RANGE");
                lastEnemyInRange = enemies; // Update the last detected enemy
            }
        }
        else
        {
            enemies = null;
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

        if (attack != null) {
            attack.Travel(enemies);
        }
    }

    // To visualise the range in testing
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
