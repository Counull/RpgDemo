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
    }

    public class Searching : EnemyStatus {
        public Searching(EnemyStatusContext context) : base(context) { }

        public override void Enter() {
            throw new System.NotImplementedException();
        }

        public override void Update() {
            throw new System.NotImplementedException();
        }

        public override void Exit() {
            throw new System.NotImplementedException();
        }

        public override bool ReadyToTransition() {
            return base.ReadyToTransition();
        }
    }

    public class Chasing : EnemyStatus {
        public Chasing(EnemyStatusContext context) : base(context) { }

        public override void Enter() {
            throw new System.NotImplementedException();
        }

        public override void Update() {
            throw new System.NotImplementedException();
        }

        public override void Exit() {
            throw new System.NotImplementedException();
        }

        public override bool ReadyToTransition() {
            return base.ReadyToTransition();
        }
    }

    public class Attacking : EnemyStatus {
        public Attacking(EnemyStatusContext context) : base(context) { }

        public override void Enter() {
            throw new System.NotImplementedException();
        }

        public override void Update() {
            throw new System.NotImplementedException();
        }

        public override void Exit() {
            throw new System.NotImplementedException();
        }

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

        public override void Exit() {
            throw new System.NotImplementedException();
        }

        public override bool ReadyToTransition() {
            return base.ReadyToTransition();
        }
    }
}