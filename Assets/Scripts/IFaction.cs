using System;
using UnityEngine;

namespace DefaultNamespace {
    public interface IFaction {
        public const string PlayerTag = "Player";
        public const string EnemyTag = "Enemy";
        Faction Faction { get; set; }
    }


    public enum Faction {
        Player,
        Enemy
    }

    public static class FactionHelper {
        public static Faction InitFaction(this IFaction faction, GameObject gameObject) {
            switch (gameObject.tag) {
                //直接访问tag属性会有性能问题
                case IFaction.PlayerTag:
                    faction.Faction = Faction.Player;
                    return Faction.Player;
                case IFaction.EnemyTag:
                    faction.Faction = Faction.Enemy;
                    return Faction.Enemy;
            }

            throw new Exception("Unknown faction");
        }
    }
}