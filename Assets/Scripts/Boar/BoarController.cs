using System;
using Player;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Boar {
    public class BoarController : EnemyController {
        [OdinSerialize] private BoarAttr _attr;

        public override string SpawnPool => "Boar";
        private EnemyStatus _status;
        private EnemyStatusContext _context;

        private void Awake() { }

        private void Start() {
            InitContext();
            _status = new Searching(_context);
            _status.Enter();
        }

        private void Update() {
            if (_status.ReadyToTransition()) {
                _status.Exit();
                _status = _status.NextStatus;
                _status.Enter();
            }

            _status.Update();
        }


        public override void BodyDisappear() {
            _context.HealthComponent.Reset();
            _context.AnimAdapter.Reset();
            base.BodyDisappear();
        }

        void InitContext() {
            _context = new EnemyStatusContext {
                Controller = this,
                AnimAdapter = GetComponentInChildren<BaseAnimAdapter>(),
                HealthComponent = GetComponent<HealthComponent>(),
                Transform = transform,
                Attr = _attr,
            };
        }

        private void OnDrawGizmos() {
            if (PoolId != 0 || InstanceId != 0) return;
            Gizmos.color = Color.yellow;
            DrawWireCircle(Vector3.zero, _attr.MaxWanderRadius);
        }

        //懒了 随地大小便
        private void DrawWireCircle(Vector3 position, float radius) {
            int segments = 30;
            float angle = 0f;
            Vector3 lastPoint = position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            for (int i = 1; i <= segments; i++) {
                angle = i * Mathf.PI * 2 / segments;
                Vector3 nextPoint = position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                Gizmos.DrawLine(lastPoint, nextPoint);
                lastPoint = nextPoint;
            }
        }
    }


    [Serializable]
    public class BoarAttr {
        [OdinSerialize] public float Speed { get; private set; }
        [OdinSerialize] public float MaxWanderRadius { get; private set; }
        [OdinSerialize] public float MaxSwitchTime { get; private set; }
    }
}