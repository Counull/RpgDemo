using UnityEngine;

namespace Damage {
    public abstract class DmgSrcComponent : MonoBehaviour, IFaction {
        [SerializeField] protected float dmg;
        [SerializeField] private Faction faction;
        public Faction Faction => faction;

        public abstract void Trigger(bool trigger);
    }
}