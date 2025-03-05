using System;
using Player;

namespace Boar {
    public class BoarController : EnemyController {
        public override string SpawnPool => "Boar";
        private EnemyStatus _status;
        private EnemyStatusContext _context;

        private void Awake() { }

        private void Start() {
            InitContext();
            _status = new Searching(_context);
            _status.Enter();
        }

        private void Update() {
            if (_status.ReadyToTransition()) {
                _status.Exit();
                _status = _status.NextStatus;
                _status.Enter();
            }

            _status.Update();
        }


        public override void BodyDisappear() {
            _context.HealthComponent.Reset();
            _context.AnimAdapter.Reset();
            base.BodyDisappear();
        }

        void InitContext() {
            _context = new EnemyStatusContext {
                Controller = this,
                AnimAdapter = GetComponentInChildren<BaseAnimAdapter>(),
                HealthComponent = GetComponent<HealthComponent>()
            };
        }
    }
}