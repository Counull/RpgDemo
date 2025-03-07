public interface IFaction {
    Faction Faction { get; }
}


public enum Faction {
    Player,
    Enemy
}

public static class FactionHelper { }