using System;
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
        const string description = "MUUUUUUUUUU";
        static bool[] requirements = new bool[]{false, true, false, false, false};
        const int LOCAL_SPRITEID = 5;
        const int LOCAL_SPRITEID_BLUEPRINT = 11;

        // Resources cost: 
        const int GOLD_COST = 1000;
        const int WOOD_COST = 5;
        const int ORE_COST = 0;
        const int CRYSTAL_COST = 0;
        const int GEM_COST = 0;



        // This needs no indata since it knows its values.
        public TrollCave() : base(name, description, requirements, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST), LOCAL_SPRITEID, LOCAL_SPRITEID_BLUEPRINT)
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
            return UI.WindowTypes.DWELLING_CARD;
        }
    }
}
