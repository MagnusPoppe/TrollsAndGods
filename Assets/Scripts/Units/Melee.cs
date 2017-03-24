using Units;

public class Melee : Unit
{
    public Melee(string name, Element element, int tier, int faction, UnitStats unitstats, Move[] moves, Ability[] abilities, Cost price) : base(name, element, tier, faction, unitstats, moves, abilities, price)
    {
    }

    public Melee()
    {
        
    }
}
