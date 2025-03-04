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
        private PlayerAnimAdapter _animAdapter;
        private PlayerMovement _playerMovement;
        private HealthComponent _healthComponent;

        private Vector3[] _enemyDetectDir;


        void Start() {
            _animAdapter = GetComponentInChildren<PlayerAnimAdapter>();
            _playerMovement = GetComponent<PlayerMovement>();
            _healthComponent = GetComponent<HealthComponent>();
            _playerMovement.PlayerMovementStatusChange += (moving) => { _animAdapter.Running = moving; };
            _healthComponent.OnDead += OnDead;
        }

        private void OnDead() {
            _animAdapter.Dead = true;
            _playerMovement.enabled = false;
            this.enabled = false;
        }


        private void Update() {
            CheckForEnemies();
        }

        private void CheckForEnemies() {
            if (_animAdapter.IsAttackingTriggered) return;

            _enemyDetectDir = UpdateEnemyDetectDirections();

            if (!_enemyDetectDir.Any(
                    dir => Physics.Raycast(transform.position + enemyDetectionOffset, dir, out var hit,
                               enemyDetectionRange)
                           && hit.collider.CompareTag("Enemy")))
                return;
            DetectedEnemyInCurrentFrame();
        }


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