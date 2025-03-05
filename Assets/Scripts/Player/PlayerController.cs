using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player {
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour {
        [SerializeField] private float enemyDetectionRange = 10.0f;
        [SerializeField] private float enemyDetectionAngle = 30.0f;
        [SerializeField] private Vector3 enemyDetectionOffset = new Vector3(0, 1, 0);
        [SerializeField] private DmgSrcComponent currentWeapon;
        private BaseAnimAdapter _animAdapter;
        private PlayerMovement _playerMovement;
        private HealthComponent _healthComponent;

        private Vector3[] _enemyDetectDir;


        void Start() {
            if (currentWeapon) currentWeapon.enabled = false;

            _animAdapter = GetComponentInChildren<BaseAnimAdapter>();
            _playerMovement = GetComponent<PlayerMovement>();
            _healthComponent = GetComponent<HealthComponent>();
            _healthComponent.OnDead += OnDead;
            _playerMovement.PlayerMovementStatusChange += (moving) => { _animAdapter.Running = moving; }; //跑步动画调度
            _animAdapter.OnAnimAttackStatusChange += (attacking) => {
                //根据动画判定攻击是否生效
                if (!currentWeapon) throw new Exception("No weapon found");
                currentWeapon.enabled = attacking;
            };
        }

        private void OnDead() {
            _animAdapter.Dead = true;
            _playerMovement.enabled = false;
            this.enabled = false;
        }


        private void Update() {
            if (CheckForEnemies()) {
                DetectedEnemyInCurrentFrame();
            }
        }

        private bool CheckForEnemies() {
            if (_animAdapter.IsAttackingTriggered) return false;
            _enemyDetectDir = UpdateEnemyDetectDirections();
            return _enemyDetectDir.Any(
                dir => Physics.Raycast(transform.position + enemyDetectionOffset, dir, out var hit,
                           enemyDetectionRange)
                       && hit.collider.CompareTag("Enemy"));
        }


        //更新检测方向
        private Vector3[] UpdateEnemyDetectDirections() {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            var ret = new[] {
                forward,
                Quaternion.Euler(0, -enemyDetectionAngle, 0) * forward,
                Quaternion.Euler(0, enemyDetectionAngle, 0) * forward,
                Quaternion.Euler(enemyDetectionAngle, 0, 0) * forward
            };
            return ret;
        }

        private void DetectedEnemyInCurrentFrame() {
            _animAdapter.TriggerAttack();
        }

        private void OnDrawGizmos() {
            if (_enemyDetectDir == null) return;
            Gizmos.color = Color.red;
            foreach (var dir in _enemyDetectDir) {
                Gizmos.DrawRay(transform.position + enemyDetectionOffset, dir * enemyDetectionRange);
            }
        }
    }
}