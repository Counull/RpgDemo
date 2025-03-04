using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player {
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private float speed = 5f;

        private PlayerInput _playerInput;
        public Vector3 CurrentFrameMovement { get; private set; }
        public bool Moving => CurrentFrameMovement != Vector3.zero;
        public Action<bool> PlayerMovementStatusChange;

        void Start() {
            _playerInput = GetComponent<PlayerInput>();
        }


        void Update() {
            if (_playerInput.InputVector == Vector2.zero) {
                if (!Moving) return;
                //如果第一次无输入那么归零CurrentFrameMovement且发送事件
                CurrentFrameMovement = Vector3.zero;
                PlayerMovementStatusChange?.Invoke(false);
                return;
            }

            var shouldInvokeEvent = !Moving;
            CurrentFrameMovement = new Vector3(_playerInput.InputVector.x, 0, _playerInput.InputVector.y) *
                                   (Time.deltaTime * speed);
            transform.position += CurrentFrameMovement;
            transform.rotation =
                Quaternion.LookRotation(new Vector3(_playerInput.InputVector.x, 0, _playerInput.InputVector.y));

            if (shouldInvokeEvent) PlayerMovementStatusChange?.Invoke(true);
        }
    }
}