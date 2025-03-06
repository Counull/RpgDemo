using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace {
    public class TriggerDmgSrcComponent : DmgSrcComponent {
        private HashSet<int> _hited = new();

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

        public override void Trigger(bool trigger) {
            enabled = trigger;
        }
    }
}