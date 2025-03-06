using Player;
using UnityEngine;

namespace Boar {
    public class EnemyStatusContext {
        public BaseAnimAdapter AnimAdapter;
        public HealthComponent HealthComponent;
        public EnemyController Controller;
        public Transform Transform;
        public BoarAttr Attr;
        public SearchingArea SearchingArea;


        public bool EnemyInAttackRange() {
            return SearchingArea.NearestPlayerDistanceSq <=
                   Attr.AttackRange * Attr.AttackRange;
        }
    }
}