using System.Collections;
using UnityEngine;

namespace Boar {
    public abstract class EnemyStatus {
        public EnemyStatus NextStatus { get; set; }
        protected EnemyStatusContext Context;

        public EnemyStatus(EnemyStatusContext context) {
            Context = context;
        }

        public abstract void Enter();

        public abstract void Update();

        public abstract void Exit();

        public virtual bool ReadyToTransition() {
            if (!Context.HealthComponent.IsDead) return false;
            NextStatus = new Dead(Context);
            return true;
        }

        protected void Move() {
            Context.Transform.position += Context.Transform.forward * Context.Attr.Speed * Time.deltaTime;
        }
    }

    public class Searching : EnemyStatus {
        private bool _moving = false;
        WaitForSeconds _waitTime;
        private bool wanding = true;
        Coroutine _wanderCoroutine;
        public Searching(EnemyStatusContext context) : base(context) { }

        public override void Enter() {
            _wanderCoroutine = Context.Controller.StartCoroutine(Wander());
        }

        public override void Update() {
            if (!_moving) return;
            if (Context.Transform.position.magnitude > Context.Attr.MaxWanderRadius) {
                Context.Transform.rotation = Quaternion.LookRotation(-Context.Transform.position, Vector3.up);
            }

            Move();
        }

        public override void Exit() {
            wanding = false;
            Context.Controller.StopCoroutine(_wanderCoroutine);
        }

        public override bool ReadyToTransition() {
            return base.ReadyToTransition();
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
        }
    }

    public class Chasing : EnemyStatus {
        public Chasing(EnemyStatusContext context) : base(context) { }

        public override void Enter() { }

        public override void Update() { }

        public override void Exit() { }

        public override bool ReadyToTransition() {
            return base.ReadyToTransition();
        }
    }

    public class Attacking : EnemyStatus {
        public Attacking(EnemyStatusContext context) : base(context) { }

        public override void Enter() { }

        public override void Update() { }

        public override void Exit() { }

        public override bool ReadyToTransition() {
            return base.ReadyToTransition();
        }
    }

    public class Dead : EnemyStatus {
        public Dead(EnemyStatusContext context) : base(context) { }

        public override void Enter() {
            Context.AnimAdapter.Dead = true;
        }

        public override void Update() { }

        public override void Exit() { }

        public override bool ReadyToTransition() {
            return base.ReadyToTransition();
        }
    }
}