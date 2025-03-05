using System;
using DefaultNamespace;
using UnityEngine;

public class DmgSrcComponent : MonoBehaviour, IFaction {
    [SerializeField] private float dmg;


    private void Start() {
        this.InitFaction(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.TryGetComponent<HealthComponent>(out var healthComponent)) return;
        if (healthComponent.Faction == Faction) return;

        //DMG
        healthComponent.CurrentHealth -= dmg;
    }

    public Faction Faction { get; set; }
}