using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{

    public GameObject gameOverUI;

    public void Update()
    {
        //gameOverUI.SetActive(true);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
