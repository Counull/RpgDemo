using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DmgSrcComponent : MonoBehaviour, IFaction {
    [SerializeField] private float dmg;
    private HashSet<int> _hited = new HashSet<int>();

    private void Awake() {
        this.InitFaction(gameObject);
    }

    private void OnDisable() {
        _hited.Clear();
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.TryGetComponent<HealthComponent>(out var healthComponent)) return;
        if (healthComponent.Faction == Faction) return;
        var id = healthComponent.GetInstanceID();
        if (!_hited.Add(id)) {
            return;
        }

        //DMG
        healthComponent.CurrentHealth -= dmg;
    }

    public Faction Faction { get; set; }
}