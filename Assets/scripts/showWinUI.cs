using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showWinUI : MonoBehaviour
{
    public GameObject gameObjectUI;
    public int counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (counter == 15)
            gameObjectUI.SetActive(true);
    }

    public void EnemyCounter()
    {
        counter++;
    }
}
