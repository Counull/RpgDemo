using System;
using UnityEngine;

namespace Player {
    public class PlayerAnimAdapter : MonoBehaviour {
        private static readonly int RunningHash = Animator.StringToHash("IsRunning");
        private static readonly int DeadHash = Animator.StringToHash("IsDead");
        private static readonly int AttackHash = Animator.StringToHash("Attack");

        public event Action<bool> OnAnimAttackStatusChange;

        Animator _animator;


        private bool _isAnimAttacking;

        public bool IsAnimAttacking {
            get => _isAnimAttacking;
            private set {
                _isAnimAttacking = value;
                if (!IsAnimAttacking) IsAttackingTriggered = false;
                OnAnimAttackStatusChange?.Invoke(value);
            }
        }

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

        public void TriggerAttack() {
            _animator.SetTrigger(AttackHash);
            IsAttackingTriggered = true;
        }


        //动画响应事假，当动画播放到攻击关键帧时表示攻击起效
        public void OnAnimAttacking(int attacking) {
            IsAnimAttacking = attacking == 1;
        }
    }
}