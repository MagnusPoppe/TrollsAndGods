using System;
using UI;
using Units;

namespace TownView
{
    /// <summary>
    /// Placeholder class for a spesific building.
    /// Belongs to the "Unknown Town".
    /// </summary>
    public class WarriorsCamp : Building
    {
        // Required values for building:
        const string name = "Warrior's Camp";
        const string description = "A camp. A camp for warriors!";
        static bool[] requirements = new bool[] { true, true, false, false, false, false, false, false, false, false, false, false };
        const int LOCAL_SPRITEID = 11;
        const int LOCAL_SPRITEID_BLUEPRINT = 23;

        // Resources cost: 
        const int GOLD_COST = 1000;
        const int WOOD_COST = 0;
        const int ORE_COST = 0;
        const int CRYSTAL_COST = 0;
        const int GEM_COST = 0;



        // This needs no indata since it knows its values.
        public WarriorsCamp() : base(name, description, requirements, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST), LOCAL_SPRITEID, LOCAL_SPRITEID_BLUEPRINT)
        {
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
