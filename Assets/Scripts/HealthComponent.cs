using System;
using DefaultNamespace;
using Sirenix.OdinInspector;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IFaction {
    public event Action OnDead;
    public event Action<float> OnHealthChange;
    private float _currentHealth;

    public bool IsDead => CurrentHealth <= 0;
    [ShowInInspector] public float MaxHealth { get; set; }

    public float CurrentHealth {
        get => _currentHealth;
        set {
            _currentHealth = value;
            OnHealthChange?.Invoke(_currentHealth);
            if (IsDead) OnDead?.Invoke();
        }
    }

    private void Start() {
        CurrentHealth = MaxHealth;
        this.InitFaction(gameObject);
    }

    public Faction Faction { get; set; }
}