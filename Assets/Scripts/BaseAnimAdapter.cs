using System;
using Boar;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player {
    public class BaseAnimAdapter : MonoBehaviour {
        private static readonly int RunningHash = Animator.StringToHash("IsRunning");
        private static readonly int DeadHash = Animator.StringToHash("IsDead");
        private static readonly int AttackHash = Animator.StringToHash("Attack");

        //当返回True时表示攻击动画起效 必须触发攻击逻辑
        public event Action<bool> OnAnimAttackValid;

        Animator _animator;


        private bool isAnimAttackingValid;

        //攻击动画起效在为True时必须触发攻击逻辑抵扣血量，当攻击关键帧结束后会重置IsAttackingTriggered
        public bool IsAnimAttackingValid {
            get => isAnimAttackingValid;
            private set {
                isAnimAttackingValid = value;
                if (!IsAnimAttackingValid) IsAttackingTriggered = false;
                OnAnimAttackValid?.Invoke(value);
            }
        }

        //是否在播放攻击动画，但是攻击动画并不一定是攻击起效
        //通过TriggerAttack()设置为true，又通过IsAnimAttacking设置为false
        public bool IsAttackingTriggered { get; private set; }


        public bool Running {
            get => _animator.GetBool(RunningHash);
            set => _animator.SetBool(RunningHash, value);
        }

        public bool Dead {
            get => _animator.GetBool(DeadHash);
            [Button]
            set {
                _animator.ResetTrigger(AttackHash);
                IsAnimAttackingValid = false;
                _animator.SetBool(DeadHash, value);
            }
        }


        public void Awake() {
            _animator = GetComponent<Animator>();
        }

        //触发攻击动画
        [Button]
        public void TriggerAttack() {
            _animator.SetTrigger(AttackHash);
            IsAttackingTriggered = true;
        }


        //动画响应事假，当动画播放到攻击关键帧时表示攻击起效
        public void OnAnimAttacking(int attacking) {
            IsAnimAttackingValid = attacking == 1;
        }

        [Button]
        public void Reset() {
            IsAnimAttackingValid = false;
            _animator.Rebind();
        }
    }
}