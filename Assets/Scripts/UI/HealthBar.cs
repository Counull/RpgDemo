using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    private HealthComponent _healthComponent;
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthText;


    [SerializeField] private bool invert;

    private Camera uiCamera;
    Camera mainCamera;

    private void Awake() { }

    public void SetHealthComponent(HealthComponent healthComponent) {
        if (_healthComponent) {
            _healthComponent.OnHealthChange -= OnHealthChange;
        }

        _healthComponent = healthComponent;
        healthComponent.OnHealthChange += OnHealthChange;
        OnHealthChange(_healthComponent.CurrentHealth);
    }

    private void OnHealthChange(float currentHealth) {
        var healthPer = currentHealth / _healthComponent.maxHealth;
        healthBar.fillAmount =healthPer ;
        healthBar.color = Color.Lerp(Color.red, Color.green, healthPer);
        healthText.text = $"{currentHealth}/{_healthComponent.maxHealth}";
    }

    private void LateUpdate() {
        /*if (invert) {
            var dir = (transform.position - mainCamera.transform.position).normalized;
            transform.LookAt(transform.position + dir);
        }
        else {
            transform.LookAt(mainCamera.transform.position);
        }*/
    }
}