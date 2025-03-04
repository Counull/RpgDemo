using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerInput : MonoBehaviour {
        private RockerController _rockerController;
        public Vector2 InputVector { get; private set; }


        private void Start() {
            _rockerController = FindFirstObjectByType<RockerController>();
        }

        
        void Update() {
            InputVector = _rockerController.outPos / _rockerController.R;
        }
    }
}