using Units;

public class Melee : Unit
{
    public Melee(string name, Element element, int tier, int faction, UnitStats unitstats, Move[] moves, Ability[] abilities, Cost price, int localID) : base(name, tier, faction, localID)
    {
    }
}
