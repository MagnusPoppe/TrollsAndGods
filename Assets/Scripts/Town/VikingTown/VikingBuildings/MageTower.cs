using System;
using UI;
using Units;

namespace TownView
{
    /// <summary>
    /// Dragon Tower spell-purchasing building belonging to Viking Town
    /// </summary>
    public class MageTower : Building
    {
        const string name = "Mage Tower";
        const string description = "Automagisk!";
        static bool[] requirements = new bool[] { true, false, false, false, false, false, false, false, false, false, false, false };
        const int LOCAL_SPRITEID = 10;
        const int LOCAL_SPRITEID_BLUEPRINT = 22;

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
        public MageTower() : base(name, description, requirements, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST), LOCAL_SPRITEID, LOCAL_SPRITEID_BLUEPRINT)
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
