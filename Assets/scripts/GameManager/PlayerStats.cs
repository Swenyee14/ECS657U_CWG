using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int currency;
    public int startcurrency = 2; 
    // Start is called before the first frame update
    void Start()
    {
        currency = startcurrency;
    }
}
