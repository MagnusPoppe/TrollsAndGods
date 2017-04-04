using Assets.Scripts.Abilities;
using UnityEngine.Rendering;

namespace Units
{
    public class StoneTroll : Ranged
    {
        private const string name = "Stone Troll";
        private const int tier = 7;
        private const int faction = 0;

        private const int attack = 22;
        private const int defence = 20;
        private const int speed = 11;
        private const int moral = 0;
        private const int luck = 0;
        private const int minDamage = 50;
        private const int maxDamage = 70;
        private const int health = 100;
        private const int initative = 5;
        private const int effectiveRange = 5;

        private const int ammo = 3;
        private const bool meleePenalty = false;

        private const int localID = 0;


        public StoneTroll() : base(name, tier, faction, ammo, meleePenalty, localID)
        {
            Element = new Earth();
            Tier = 7;
            Faction = 0;


            Unitstats = new UnitStats(attack,defence,speed,moral,luck,minDamage,maxDamage,health,initative,effectiveRange);

            Moves = new Move[] { new TreeSwing(), new BoulderThrow()};
            Price = new Cost(100, 1, 1, 1, 1);
            Ability = new StoneSkin();
        }
    }
}
