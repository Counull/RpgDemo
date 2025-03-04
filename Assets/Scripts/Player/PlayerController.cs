using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour {
        private PlayerAnimAdapter _animAdapter;
        private PlayerMovement _playerMovement;
        private HealthComponent _healthComponent;

        private LinkedList<HealthComponent> _enemiesInRange = new LinkedList<HealthComponent>();

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
        }


        private void Update() { }

        private void OnTriggerEnter(Collider other) {
          
            
        }

        private void OnTriggerStay(Collider other) { }
    }
}