using System;
using UnityEngine;

namespace Boar {
    /// <summary>
    ///     根据Trigger判断玩家是否在追逐范围内
    /// </summary>
    public class SearchingArea : MonoBehaviour {
        [SerializeField] private string targetTag = TagAndLayerHelper.Tags.Player;

        public Transform NearestPlayerTrans { get; private set; }
        public float NearestPlayerDistanceSq { get; private set; } = float.MaxValue;


        public void Reset() {
            NearestPlayerTrans = null;
            NearestPlayerDistanceSq = float.MaxValue;
        }


        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag(targetTag)) return;
            OntTriggeredPlayerChange?.Invoke(true, other.transform);
        }

        private void OnTriggerExit(Collider other) {
            if (!other.CompareTag(targetTag)) return;
            if (other.transform == NearestPlayerTrans) NearestPlayerInvalid();

            OntTriggeredPlayerChange?.Invoke(false, other.transform);
        }

        private void OnTriggerStay(Collider other) {
            if (!other.CompareTag(targetTag)) return;

            //如果玩家死亡，那么就不再是最近的玩家 其实这里写的很乱，按理说应该关闭玩家身上的碰撞体但是处理哪些逻辑又很麻烦
            //这里这么写 未来会有很大的麻烦 尤其是针对OntTriggeredPlayerChange
            if (!other.TryGetComponent<HealthComponent>(out var healthComponent)) return;
            if (healthComponent.IsDead) {
                //如果玩家死亡就不计算后续逻辑了
                if (other.transform == NearestPlayerTrans)
                    //死亡玩家是最近玩家那么最近玩家无效
                    NearestPlayerInvalid();

                return;
            }

            var distanceSq = (other.transform.position - transform.position).sqrMagnitude;
            if (distanceSq < NearestPlayerDistanceSq && other.transform != NearestPlayerTrans) {
                //如果最近玩家发生变化
                NearestPlayerDistanceSq = distanceSq;
                NearestPlayerTrans = other.transform;
                OnNearestPlayerChange?.Invoke(NearestPlayerTrans);
                return; //这里写这个更新距离且return是为了保证在OnNearestPlayerChange被触发之前所有数据得到更新
            }

            //持续更新最近玩家的距离
            NearestPlayerDistanceSq = distanceSq;
        }

        /// <summary>
        ///     玩家是否进出追逐范围
        /// </summary>
        public event Action<bool, Transform> OntTriggeredPlayerChange;

        /// <summary>
        ///     当所追逐的最近的玩家改变
        /// </summary>
        public event Action<Transform> OnNearestPlayerChange;

        /// <summary>
        ///     当最近玩家无效时调用
        /// </summary>
        private void NearestPlayerInvalid() {
            NearestPlayerTrans = null;
            NearestPlayerDistanceSq = float.MaxValue;
            OnNearestPlayerChange?.Invoke(null);
        }
    }
}