using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;

        if (target == null)
        {
            target = GameObject.FindWithTag("Enemies").transform;
        }

    }
    public void UpdateHealthBar(float health, float maxHealth)
    {
        slider.value = health / maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.rotation = mainCamera.transform.rotation;
            transform.position = target.position + offset;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
