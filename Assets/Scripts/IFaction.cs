using System;
using UnityEngine;

namespace DefaultNamespace {
    public interface IFaction {
        Faction Faction { get; }
    }


    public enum Faction {
        Player,
        Enemy
    }

    public static class FactionHelper {
  
    }
}