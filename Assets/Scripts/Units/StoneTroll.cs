namespace Units
{
    public class StoneTroll : Ranged
    {

        public StoneTroll(string name, Element element, int tier, int faction, UnitStats unitstats, int ammo, bool meleePenalty, Move[] moves, Ability[] abilities, Cost price) : base(name, element, tier, faction, unitstats,moves,abilities, ammo, meleePenalty, price)
        {
        }

        public StoneTroll()
        {
            Name = "Stone Troll";
            Element = new Earth();
            Tier = 7;
            Faction = 0;

            int attack = 22;
            int defence = 20;
            int speed = 11;
            int moral = 0;
            int luck = 0;
            int minDamage = 50;
            int maxDamage = 70;
            int health = 100;
            int initative = 5;
            int effectiveRange = 5;

            Unitstats = new UnitStats(attack,defence,speed,moral,luck,minDamage,maxDamage,health,initative,effectiveRange);

            Ammo = 3;
            MeleePenalty = false;
            Moves = new Move[] { new TreeSwing(), new BoulderThrow()};
            Price = new Cost(100, 10, 5, 0, 0);
            //ability = new StoneSkin(); TODO: make ability
        }
    }
}
