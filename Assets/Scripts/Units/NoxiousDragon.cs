using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public class NoxiousDragon : Melee
    {
        public NoxiousDragon(string name, Element element, int tier, int faction, UnitStats unitstats, Move[] moves, Ability[] abilities) : base(name, element, tier, faction, unitstats, moves, abilities)
        {
        }

        public NoxiousDragon()
        {
            Name = "Noxious Dragon";
            Element = new Earth();
            Tier = 8;
            Faction = 0;

            int attack = 30;
            int defence = 30;
            int speed = 11;
            int moral = 0;
            int luck = 0;
            int minDamage = 100;
            int maxDamage = 150;
            int health = 500;
            int initative = 6;
            int effectiveRange = 0;

            Unitstats = new UnitStats(attack, defence, speed, moral, luck, minDamage, maxDamage, health, initative, effectiveRange);
            
            Moves = new Move[] { }; //todo make moves
            //ability = new BreathAttack(); TODO: make ability
        }
    }
}
