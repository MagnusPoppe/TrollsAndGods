using System;
using UI;
using Units;

namespace TownView
{
    /// <summary>
    /// Placeholder class for a spesific building.
    /// Belongs to the "Unknown Town".
    /// </summary>
    public class TrainingCamp : UnitBuilding
    {
        // Required values for building:
        const string name = "Training Camp";
        const string description = "Train around the campfire!";
        static bool[] requirements = new bool[] { false, true, false, false, false, false, false, false, false, false, false, false };
        const int LOCAL_SPRITEID = 9;
        const int LOCAL_SPRITEID_BLUEPRINT = 21;

        // Resources cost: 
        const int GOLD_COST = 1000;
        const int WOOD_COST = 0;
        const int ORE_COST = 0;
        const int CRYSTAL_COST = 0;
        const int GEM_COST = 0;



        // This needs no indata since it knows its values.
        public TrainingCamp() : base(name, description, requirements, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST), LOCAL_SPRITEID, LOCAL_SPRITEID_BLUEPRINT)
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
