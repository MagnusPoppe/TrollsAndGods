using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UI;
using Units;

namespace TownView
{
    /// <summary>
    /// Placeholder class for a spesific building.
    /// Belongs to the "Unknown Town".
    /// </summary>
    public class TrollCave : Building, UnitPlayingCard
    {
        // Required values for building:
        const string name = "Troll Cave";
        const bool[] requirements = null;
        const int LOCAL_SPRITEID = 5;

        // Resources cost: 
        const int GOLD_COST = 0;
        const int WOOD_COST = 0;
        const int ORE_COST = 0;
        const int CRYSTAL_COST = 0;
        const int GEM_COST = 0;



        // This needs no indata since it knows its values.
        public TrollCave() : base(name, requirements, new Resources(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST), LOCAL_SPRITEID)
        {
        }

        public int GetImage()
        {
            throw new NotImplementedException();
        }

        public int GetAttack()
        {
            // TODO: Unit.getattack();
            return GetAttack();
        }

        public int GetDefense()
        {
            return GetDefense();
        }

        public int GetMagic()
        {
            return GetMagic();
        }

        public int GetSpeed()
        {
            return GetSpeed();
        }

        public int GetHealthPoints()
        {
            return GetHealthPoints();
        }

        public string GetUnitName()
        {
            return GetUnitName();
        }

        public Move[] GetMoves()
        {
            return GetMoves();
        }

        public string GetAbility()
        {
            throw new NotImplementedException();
        }

        public int UIType()
        {
            return UI.WindowTypes.BUILDING_PLAYING_CARD;
        }
    }
}
