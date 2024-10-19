using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{

    public Transform enemies;
    public float range = 7f;
    public string enemyTag = "Enemies";
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateEnemy", 0f, 1f);
    }

    void UpdateEnemy()
    {
        List<GameObject> allEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag(enemyTag));
        allEnemies.RemoveAll(enemy => Vector3.Distance(transform.position, enemy.transform.position) > range); // Remove enemies out of range

        if (allEnemies.Count > 0)
        {
            GameObject nearestEnemy = allEnemies.Aggregate((closest, next) =>
                Vector3.Distance(transform.position, next.transform.position) <
                Vector3.Distance(transform.position, closest.transform.position) ? next : closest);

            enemies = nearestEnemy.transform;
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
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
