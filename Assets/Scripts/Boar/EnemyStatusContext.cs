using System;
using Sirenix.Serialization;
using UnityEngine;

namespace Boar {
    /// <summary>
    ///     用于在状态中共享数据
    /// </summary>
    public class EnemyStatusContext {
        public BoarAttr Attr;
        public Transform Transform;
        public BaseAnimAdapter AnimAdapter;
        public EnemyController Controller;
        public HealthComponent HealthComponent;
        public SearchingArea SearchingArea;
        
        public bool EnemyInAttackRange() {
            return SearchingArea.NearestPlayerDistanceSq <=
                   Attr.AttackRange * Attr.AttackRange;
        }
    }


    /// <summary>
    ///     野猪的基础数据
    /// </summary>
    [Serializable]
    public class BoarAttr {
        [OdinSerialize] public float Speed { get; private set; }
        [OdinSerialize] public float MaxWanderRadius { get; private set; }
        [OdinSerialize] public float MaxSwitchTime { get; private set; }
        [OdinSerialize] public float AttackRange { get; private set; }
        [OdinSerialize] public float AttackInterval { get; private set; }
    }
}