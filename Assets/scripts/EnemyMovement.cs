using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 6f;
    private Transform target;
    private int positionIndex = 0;

    void Start()
    {
        // reference to the waypoint
        target = Positions.positions[0];
    }

    void Update()
    {

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            GetNextPosition();
        }
    }

    void GetNextPosition()
    {
        if (positionIndex == Positions.positions.Length - 1)
        {
            Destroy(gameObject);
            return;
        }

        positionIndex++;
        target = Positions.positions[positionIndex];
    }
}
