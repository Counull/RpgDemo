using System;
using DefaultNamespace;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthComponent : MonoBehaviour, IFaction {
    public event Action OnDead;
    public event Action<float> OnHealthChange;
    private float _currentHealth;

    public bool IsDead => CurrentHealth <= 0;
    [SerializeField] public float maxHealth;

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

    private void Start() {
        Reset();
    }

    public void Reset() {
        CurrentHealth = maxHealth;
    }

    [SerializeField] Faction faction;

    public Faction Faction => faction;
}