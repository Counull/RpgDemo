using UnityEngine;

namespace Player {
    public class PlayerInput : MonoBehaviour {
        private RockerController _rockerController;
        public Vector2 InputVector { get; private set; }


        private void Start() {
            _rockerController = FindFirstObjectByType<RockerController>();
        }


        private void Update() {
            InputVector = _rockerController.outPos / _rockerController.R;
        }
    }
}