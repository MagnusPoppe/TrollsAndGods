using System;
using UI;
using Units;

namespace TownView
{
    /// <summary>
    /// Workshop builder building belonging to Viking Town
    /// </summary>
    public class Workshop : Building
    {
        const string name = "Workshop";
        const string description = "Buy buildings at great prices!!";
        static bool[] requirements = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false };
        const int LOCAL_SPRITEID = 12;
        const int LOCAL_SPRITEID_BLUEPRINT = -1;

        /// <summary>
        /// Resource cost for this building
        /// </summary>
        const int GOLD_COST = 1000;
        const int WOOD_COST = 0;
        const int ORE_COST = 0;
        const int CRYSTAL_COST = 0;
        const int GEM_COST = 0;



        /// <summary>
        /// Default constructor
        /// </summary>
        public Workshop() : base(name, description, requirements, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST), LOCAL_SPRITEID, LOCAL_SPRITEID_BLUEPRINT)
        {
        }

        /// <summary>
        /// Override class to tell which card window this building uses
        /// </summary>
        /// <returns>Integer for which window type to display in the game</returns>
        protected override int GetUIType()
        {
            return UI.WindowTypes.TOWN_HALL_CARD;
        }
    }
}
