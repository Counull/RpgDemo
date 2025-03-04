using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class HealthComponent : MonoBehaviour {
    public event Action OnDead;
    private float _currentHealth;
    
   public bool IsDead => CurrentHealth <= 0;
    [ShowInInspector] public float MaxHealth { get; set; }


    public float CurrentHealth {
        get => _currentHealth;
        set {
            _currentHealth = value;
            if (IsDead) OnDead?.Invoke();
        }
    }

    private void Start() {
        CurrentHealth = MaxHealth;
    }
}