namespace Units
{
    public class StoneTroll : Ranged
    {

        
        Move[] moves;
        Ability ability;

        public StoneTroll(string name, Element element, int tier, int faction, UnitStats unitstats, int ammo, bool meleePenalty, Move[] moves, Ability ability) : base(name, element, tier, faction, unitstats, ammo, meleePenalty)
        {
            this.moves = moves;
            this.ability = ability;
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
            moves = new Move[] { new TreeSwing(), new BoulderThrow()};
            //ability = new StoneSkin(); TODO: make ability
        }
    }
}
