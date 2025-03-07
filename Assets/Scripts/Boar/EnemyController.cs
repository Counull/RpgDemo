using System;
using Sirenix.OdinInspector;

namespace Boar {
    public abstract class EnemyController : SerializedMonoBehaviour {
        public int PoolId { get; set; }
        public int InstanceId { get; set; }

        /// <summary>
        ///     当野猪死亡后触发此事件以让spawner重新生成野猪
        /// </summary>
        public event Action<int, int> ShouldRespawn;

        public virtual void BodyDisappear() {
            gameObject.SetActive(false);
            ShouldRespawn?.Invoke(PoolId, InstanceId);
        }
    }
}