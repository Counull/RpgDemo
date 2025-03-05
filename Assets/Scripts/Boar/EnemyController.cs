using System;
using UnityEngine;

namespace Boar {
    public abstract class EnemyController : MonoBehaviour {
        public abstract string SpawnPool { get; }
        public int PoolId { get; set; }
        public int InstanceId { get; set; }

        public event Action<int, int> ShouldRespawn;


        public virtual void BodyDisappear() {
            gameObject.SetActive(false);
            ShouldRespawn?.Invoke(PoolId, InstanceId);
        }
    }
}