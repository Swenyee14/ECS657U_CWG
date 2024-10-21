using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class stores an array of all child transforms under this GameObject
public class Positions : MonoBehaviour
{
    public static Transform[] positions;

    // gets all the children of the position game object
    private void Awake()
    {
        positions = new Transform[transform.childCount];
        for ( int i = 0; i < positions.Length; i++)
        {
            positions[i] = transform.GetChild(i);
        }
    }
}
