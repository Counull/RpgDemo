using System;
using System.Collections.Generic;
using Player;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Boar {
    public class BoarController : EnemyController {
        [OdinSerialize] private BoarAttr _attr;


        private EnemyStatus _status;
        private EnemyStatusContext _context;
        HashSet<HealthComponent> _healthComponents = new();
        private DmgSrcComponent dmgSrcComponent;

        private void Awake() {
            InitContext();
            dmgSrcComponent = GetComponentInChildren<DmgSrcComponent>();
            _context.AnimAdapter.OnAnimAttackValid += (attacking) => {
                //根据动画判定攻击是否生效
                dmgSrcComponent.Trigger(attacking);
            };
        }

        private void Start() {
            _status = new Searching(_context);
            _status.Enter(); //规避在Spawn的时候子物体还未Awake的情况
        }

        private void OnEnable() {
            _status?.Enter();
        }


        private void Update() {
            if (_status.ReadyToTransition()) {
                _status.Exit();
                _status = _status.NextStatus;
                _status.Enter();
            }

            _status.Update();
        }


        void InitContext() {
            _context = new EnemyStatusContext {
                Controller = this,
                AnimAdapter = GetComponentInChildren<BaseAnimAdapter>(),
                SearchingArea = GetComponentInChildren<SearchingArea>(),
                HealthComponent = GetComponent<HealthComponent>(),
                Transform = transform,
                Attr = _attr,
            };
        }

        public override void BodyDisappear() {
            _status?.Exit();
            _status = new Searching(_context);
            _context.HealthComponent.Reset();
            _context.AnimAdapter.Reset();
            _context.SearchingArea.Reset();
            base.BodyDisappear();
        }


        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            DrawWireCircle(transform.position, _attr.AttackRange);
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
        [OdinSerialize] public float AttackRange { get; private set; }
        [OdinSerialize] public float AttackInterval { get; private set; }
    }
}