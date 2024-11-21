using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }
    public void UpdateHealthBar(float health, float maxHealth)
    {
        slider.value = health/maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = camera.transform.rotation;
        transform.position = target.position + offset;
    }
}
