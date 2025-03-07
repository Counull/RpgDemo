using System.Collections.Generic;
using UnityEngine;

namespace Damage {
    /// <summary>
    ///     根据附着的Trigger和动画关键帧触发伤害
    /// </summary>
    public class TriggerDmgSrcComponent : DmgSrcComponent {
        private readonly HashSet<int> _hited = new();

        private void OnDisable() {
            _hited.Clear();
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.TryGetComponent<HealthComponent>(out var healthComponent)) return;
            if (healthComponent.Faction == Faction) return;
            var id = healthComponent.GetInstanceID();
            if (!_hited.Add(id)) return;

            //DMG
            healthComponent.CurrentHealth -= dmg;
        }

        public override void Trigger(bool trigger) {
            enabled = trigger;
        }
    }
}