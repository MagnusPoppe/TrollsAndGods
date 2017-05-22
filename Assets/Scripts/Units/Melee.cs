using Units;
using Abilities;
/// <summary>
///  Superclass for melee units
/// </summary>
public class Melee : Unit
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="name">Name of the unit</param>
    /// <param name="element">Element of the unit</param>
    /// <param name="tier">The units tier</param>
    /// <param name="faction">The faction this unit belongs to</param>
    /// <param name="unitstats">The units given stats</param>
    /// <param name="moves">This units moves</param>
    /// <param name="abilities">This unit's ability</param>
    /// <param name="price">This unit's price</param>
    /// <param name="localID">The local sprite id for thisu nit</param>
    public Melee(string name, Element element, int tier, int faction, UnitStats unitstats, Move[] moves, Ability[] abilities, Cost price, int localID) : base(name, tier, faction, localID)
    {
    }
}
