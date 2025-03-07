using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IFaction {
    [SerializeField] public float maxHealth;

    [SerializeField] private Faction faction;
    private float _currentHealth;

    public bool IsDead => CurrentHealth <= 0;

    [ShowInInspector]
    public float CurrentHealth {
        get => _currentHealth;

        set {
            if (value < 0) value = 0;
            _currentHealth = value;
            OnHealthChange?.Invoke(_currentHealth);
            if (IsDead) OnDead?.Invoke();
        }
    }

    public void Reset() {
        CurrentHealth = maxHealth;
    }

    private void Start() {
        Reset();
    }

    public Faction Faction => faction;
    public event Action OnDead;
    public event Action<float> OnHealthChange;
}