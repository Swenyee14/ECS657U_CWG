using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    public GameObject gameObjectUI;
    float CTime = 0f;
    float STime = 52f;

    // Start is called before the first frame update
    void Start()
    {
        CTime = STime;
    }

    // Update is called once per frame
    void Update()
    {
        CTime -= 1 * Time.deltaTime;

        if (CTime <= 0)
        {
            gameObjectUI.SetActive(true);

        }
    }
}
