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
        const int LOCAL_SPRITEID_BLUEPRINT = 10;

        // Resources cost: 
        const int GOLD_COST = 1000;
        const int WOOD_COST = 5;
        const int ORE_COST = 5;
        const int CRYSTAL_COST = 5;
        const int GEM_COST = 5;



        // This needs no indata since it knows its values.
        public TrollCave() : base(name, requirements, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST), LOCAL_SPRITEID, LOCAL_SPRITEID_BLUEPRINT)
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

        /// <summary>
        /// Override class to tell which card window this building uses
        /// </summary>
        /// <returns>Integer for which window type to display in the game</returns>
        protected override int GetUIType()
        {
            return UI.WindowTypes.DWELLING_PLAYING_CARD;
        }
    }
}
