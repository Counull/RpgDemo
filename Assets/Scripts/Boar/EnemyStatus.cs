using System.Collections;
using UnityEngine;

namespace Boar {
    public abstract class EnemyStatus {
        public EnemyStatus NextStatus { get; set; }
        protected EnemyStatusContext Context;

        public EnemyStatus(EnemyStatusContext context) {
            Context = context;
        }

        public virtual void Enter() {
#if UNITY_EDITOR
            Debug.Log($"Enter:{GetType().Name}");
#endif
        }

        public abstract void Update();

        public virtual void Exit() {
#if UNITY_EDITOR
            Debug.Log($"Exit:{GetType().Name}");
#endif
        }

        public virtual bool ReadyToTransition() {
            if (!Context.HealthComponent.IsDead) return false;
            NextStatus = new Dead(Context);
            return true;
        }

        protected void Move() {
            Context.Transform.position += Context.Transform.forward * (Context.Attr.Speed * Time.deltaTime);
        }
    }

    public class Searching : EnemyStatus {
        private bool _moving = false;
        WaitForSeconds _waitTime;
        private bool wanding = true;
        Coroutine _wanderCoroutine;
        public Searching(EnemyStatusContext context) : base(context) { }

        public override void Enter() {
            base.Enter();
            _wanderCoroutine = Context.Controller.StartCoroutine(Wander());
        }

        public override void Update() {
            if (!_moving) return;
            if (Context.Transform.position.magnitude > Context.Attr.MaxWanderRadius) {
                //超出范围转向向中心点
                Context.Transform.rotation = Quaternion.LookRotation(-Context.Transform.position, Vector3.up);
            }

            Move();
        }

        public override void Exit() {
            base.Exit();
            wanding = false;
            if (_wanderCoroutine != null)
                Context.Controller.StopCoroutine(_wanderCoroutine);
        }

        public override bool ReadyToTransition() {
            if (base.ReadyToTransition()) {
                return true;
            }

            if (!Context.SearchingArea.NearestPlayerTrans) return false;
            NextStatus = new Chasing(Context);
            return true;
        }


        IEnumerator Wander() {
            while (wanding) {
                _moving = false;
                Context.AnimAdapter.Running = false;
                _waitTime = new WaitForSeconds(Random.value * Context.Attr.MaxSwitchTime);
                yield return _waitTime;
                _moving = true;
                Context.AnimAdapter.Running = true;
                _waitTime = new WaitForSeconds(Random.value * Context.Attr.MaxSwitchTime);
                Vector2 randomVector = Random.insideUnitCircle;
                Context.Transform.rotation =
                    Quaternion.LookRotation(new Vector3(randomVector.x, 0, randomVector.y), Vector3.up);
                yield return _waitTime;
            }

            _wanderCoroutine = null;
        }
    }

    public class Chasing : EnemyStatus {
        public Chasing(EnemyStatusContext context) : base(context) { }

        float _attackRangeSq;

        public override void Enter() {
            base.Enter();
            _attackRangeSq = Context.Attr.AttackRange * Context.Attr.AttackRange;
            Context.AnimAdapter.Running = true;
        }

        public override void Update() {
            if (!Context.SearchingArea.NearestPlayerTrans) return;
            var targetDir = Context.SearchingArea.NearestPlayerTrans.position - Context.Transform.position;

            Context.Transform.rotation = Quaternion.LookRotation(targetDir, Vector3.up);
            Move();
        }

        public override void Exit() {
            base.Exit();
            Context.AnimAdapter.Running = false;
        }

        public override bool ReadyToTransition() {
            if (base.ReadyToTransition()) {
                return true;
            }

            if (Context.SearchingArea.NearestPlayerTrans) {
                if (!Context.EnemyInAttackRange()) return false; //如果距离不够就继续追
                NextStatus = new Attacking(Context); // 否则进入攻击状态
                return true;
            }

            //丢失目标继续搜索
            NextStatus = new Searching(Context);
            return true;
        }
    }

    public class Attacking : EnemyStatus {
        public Attacking(EnemyStatusContext context) : base(context) { }
        float _attackRangeSq;

        public override void Enter() {
            base.Enter();
            _attackRangeSq = Context.Attr.AttackRange * Context.Attr.AttackRange;
        }

        public override void Update() {
            if (Context.AnimAdapter.IsAttackingTriggered) return;
            var targetDir = Context.SearchingArea.NearestPlayerTrans.position - Context.Transform.position;
            Context.Transform.rotation = Quaternion.LookRotation(targetDir, Vector3.up);
            Context.AnimAdapter.TriggerAttack();
        }

        public override void Exit() {
            base.Exit();
        }

        public override bool ReadyToTransition() {
            if (base.ReadyToTransition()) {
                return true;
            }

            if (Context.AnimAdapter.IsAttackingTriggered) return false; //如果还在攻击就不切换

            if (!Context.SearchingArea.NearestPlayerTrans) {
                NextStatus = new Searching(Context);
                return true;
            }

            if (Context.EnemyInAttackRange()) return false;
            NextStatus = new Chasing(Context);
            return true;
        }
    }

    public class Dead : EnemyStatus {
        public Dead(EnemyStatusContext context) : base(context) { }

        public override void Enter() {
            base.Enter();
            Context.AnimAdapter.Dead = true;
            Context.Controller.Invoke(nameof(Context.Controller.BodyDisappear), 3f);
        }


        public override void Update() { }

        public override void Exit() {
            base.Exit();
        }

        public override bool ReadyToTransition() {
            return false;
        }
    }
}