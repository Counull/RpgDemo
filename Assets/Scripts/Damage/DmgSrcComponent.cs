using System;
using System.Collections.Generic;
using DefaultNamespace;
using Sirenix.Serialization;
using UnityEngine;

public abstract class DmgSrcComponent : MonoBehaviour, IFaction {
    [SerializeField] protected float dmg;
    [SerializeField] Faction faction;
    public Faction Faction => faction;

    public abstract void Trigger(bool trigger);
}