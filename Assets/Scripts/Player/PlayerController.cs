using System;
using Damage;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour {
        [SerializeField] private float enemyDetectionRange = 10.0f;
        [SerializeField] private float enemyDetectionAngle = 30.0f;
        [SerializeField] private Vector3 enemyDetectionOffset = new(0, 1, 0);
        [SerializeField] private DmgSrcComponent currentWeapon;
        private readonly Collider[] _hitColliders = new Collider[10]; // 预分配的数组

        private BaseAnimAdapter _animAdapter;
        private Vector3[] _enemyDetectDir;
        private HealthComponent _healthComponent;
        private PlayerMovement _playerMovement;

        private void Awake() {
            if (!currentWeapon) throw new Exception("No weapon found");
            _animAdapter = GetComponentInChildren<BaseAnimAdapter>();
            _playerMovement = GetComponent<PlayerMovement>();
            _healthComponent = GetComponent<HealthComponent>();
        }

        private void Start() {
            _healthComponent.OnDead += OnDead;
            _playerMovement.PlayerMovementStatusChange += moving => { _animAdapter.Running = moving; }; //跑步动画调度
            _animAdapter.OnAnimAttackValid += attacking => {
                //根据动画判定攻击是否生效
                currentWeapon.Trigger(attacking);
            };
        }


        private void Update() {
            if (CheckForEnemies()) _animAdapter.TriggerAttack();
        }

        private void OnDead() {
            _animAdapter.Dead = true;
            _playerMovement.enabled = false;
            enabled = false;
        }


        /// <summary>
        ///     查看怪物是否在攻击范围内
        /// </summary>
        /// <returns></returns>
        private bool CheckForEnemies() {
            if (_animAdapter.IsAttackingTriggered) return false;

            var r = enemyDetectionRange / 2;
            var origin = transform.position + enemyDetectionOffset;
            origin += transform.forward * r;
            var hitCount =
                Physics.OverlapSphereNonAlloc(origin, r, _hitColliders, TagAndLayerHelper.Instance.EnemyLayerMask);
            if (hitCount <= 0) return false;
            for (var i = 0; i < hitCount; i++)
                if (_hitColliders[i].TryGetComponent<HealthComponent>(out var healthComponent) &&
                    !healthComponent.IsDead && healthComponent.Faction != currentWeapon.Faction)
                    return true; //有活着的敌人在攻击范围内


            return false;
        }
    }
}