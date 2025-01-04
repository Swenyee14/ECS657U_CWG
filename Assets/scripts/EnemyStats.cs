using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats", order = 1)]
public class EnemyStats : ScriptableObject
{
    [Header("Health")]
    public float standardHealth;
    public float fastHealth;
    public float tankHealth;

    [Header("Speed")]
    public float standardSpeed;
    public float fastSpeed;
    public float tankSpeed;
}
