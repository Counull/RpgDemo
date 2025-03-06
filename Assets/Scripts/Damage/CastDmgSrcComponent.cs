using System;
using UnityEngine;

namespace DefaultNamespace {
    public class CastDmgSrcComponent : DmgSrcComponent {
        Collider[] _colliders = new Collider[4];
        public Vector3 point1;
        public Vector3 point2;
        public float radius;
        [SerializeField] private LayerMask targetLayerMask;

        public override void Trigger(bool trigger) {
            if (!trigger) return;
        
            var worldPoint1 = transform.TransformPoint(point1);
            var worldPoint2 = transform.TransformPoint(point2);
            var hitCount =
                Physics.OverlapCapsuleNonAlloc(worldPoint1, worldPoint2, radius, _colliders, targetLayerMask);
            if (hitCount == 0) return;
            foreach (var target in _colliders) {
                if (!target) continue;
                if (!target.TryGetComponent(out HealthComponent healthComponent)) continue;
                if (healthComponent.Faction == Faction) continue;
                healthComponent.CurrentHealth -= dmg;
            }
        }


        private void OnDrawGizmos() {
          
            var worldPoint1 = transform.TransformPoint(point1);
            var worldPoint2 = transform.TransformPoint(point2);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(worldPoint1, radius);
            Gizmos.DrawWireSphere(worldPoint2, radius);
            Gizmos.DrawLine(worldPoint1, worldPoint2);
        }
    }
}