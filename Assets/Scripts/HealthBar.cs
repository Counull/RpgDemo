using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private bool invert;

    private Camera uiCamera;
    Camera mainCamera;

    private void Awake() {
        healthComponent.OnHealthChange += OnHealthChange;
        mainCamera = Camera.main;
        uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
    }

    private void OnHealthChange(float currentHealth) {
        healthBar.fillAmount = currentHealth / healthComponent.maxHealth;
        healthText.text = $"{currentHealth}/{healthComponent.maxHealth}";
    }

    private void LateUpdate() {
        if (invert) {
            var dir = (transform.position - mainCamera.transform.position).normalized;
            transform.LookAt(transform.position + dir);
        }
        else {
            transform.LookAt(mainCamera.transform.position);
        }

       
       
    }
}