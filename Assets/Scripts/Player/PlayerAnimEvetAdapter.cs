using System;
using UnityEngine;

namespace Player {
    public class PlayerAnimAdapter : MonoBehaviour {
        private static readonly int RunningHash = Animator.StringToHash("IsRunning");
        private static readonly int DeadHash = Animator.StringToHash("IsDead");
        private static readonly int AttackHash = Animator.StringToHash("Attack");

        Animator _animator;


        public bool IsAnimAttacking { get; private set; }

        public bool Running {
            get => _animator.GetBool(RunningHash);
            set => _animator.SetBool(RunningHash, value);
        }

        public bool Dead {
            get => _animator.GetBool(DeadHash);
            set => _animator.SetBool(DeadHash, value);
        }

        public bool IsAttackingTriggered { get; set; }

        public void Awake() {
            _animator = GetComponent<Animator>();
        }

        public void TriggerAttacking() {
            _animator.SetTrigger(AttackHash);
            IsAttackingTriggered = true;
        }

        public void OnAnimAttacking(int attacking) {
            IsAnimAttacking = attacking == 1;
            if (!IsAnimAttacking) IsAttackingTriggered = false;
        }
    }
}