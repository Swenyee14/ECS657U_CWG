using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showEndUI : MonoBehaviour
{

    public GameObject gameObjectUI;

    public void NeededMethod() //sets inactive object to become active
    {
        gameObjectUI.SetActive(true);
        
    }
}
