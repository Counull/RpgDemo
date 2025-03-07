using System.Collections.Generic;
using Damage;
using Sirenix.Serialization;
using UnityEngine;

namespace Boar {
    /// <summary>
    ///     野猪行为，内置状态管理野猪行为
    /// </summary>
    public class BoarController : EnemyController {
        [OdinSerialize] private BoarAttr _attr;


        private EnemyStatus _status;
        private EnemyStatusContext _context;
        private HashSet<HealthComponent> _healthComponents = new();
        private DmgSrcComponent dmgSrcComponent;

        private void Awake() {
            InitContext();
            dmgSrcComponent = GetComponentInChildren<DmgSrcComponent>();
            _context.AnimAdapter.OnAnimAttackValid += attacking => {
                //根据动画判定攻击是否生效
                dmgSrcComponent.Trigger(attacking);
            };
        }

        private void Start() {
            _status = new Searching(_context);
            _status.Enter(); //规避在Spawn的时候子物体还未Awake的情况
        }


        /// <summary>
        ///     更新状态，详见<see cref="EnemyStatus" />
        /// </summary>
        private void Update() {
            if (_status.ReadyToTransition()) {
                _status.Exit();
                _status = _status.NextStatus;
                _status.Enter();
            }

            _status.Update();
        }

        private void OnEnable() {
            _status?.Enter();
        }


        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            DrawWireCircle(transform.position, _attr.AttackRange);
            if (PoolId != 0 || InstanceId != 0) return;
            Gizmos.color = Color.yellow;
            DrawWireCircle(Vector3.zero, _attr.MaxWanderRadius);
        }


        private void InitContext() {
            _context = new EnemyStatusContext {
                Controller = this,
                AnimAdapter = GetComponentInChildren<BaseAnimAdapter>(),
                SearchingArea = GetComponentInChildren<SearchingArea>(),
                HealthComponent = GetComponent<HealthComponent>(),
                Transform = transform,
                Attr = _attr
            };
        }

        /// <summary>
        ///     当野猪死亡时，播放完死亡动画后的一段时间，重置各组件状态
        ///     基类调用包含设置Active
        ///     和通知的<see cref="EnemySpawner" />回收的事件
        /// </summary>
        public override void BodyDisappear() {
            _status?.Exit();
            _status = new Searching(_context);
            _context.HealthComponent.Reset();
            _context.AnimAdapter.Reset();
            _context.SearchingArea.Reset();
            base.BodyDisappear();
        }

        //懒了 随地大小便
        private void DrawWireCircle(Vector3 position, float radius) {
            var segments = 30;
            var angle = 0f;
            var lastPoint = position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            for (var i = 1; i <= segments; i++) {
                angle = i * Mathf.PI * 2 / segments;
                var nextPoint = position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                Gizmos.DrawLine(lastPoint, nextPoint);
                lastPoint = nextPoint;
            }
        }
    }
}