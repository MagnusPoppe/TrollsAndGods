using System;
using UI;
using Units;

namespace TownView
{
    /// <summary>
    /// Beast Rider's Guild Unit building belonging to Viking Town
    /// </summary>
    public class BeastRidersGuild : UnitBuilding
    {
        const string name = "Beast Rider's Guild";
        const string description = "Buy beast riders here!";
        static bool[] requirements = new bool[] { false, true, false, false, false, false, false, false, false, false, false, false };
        const int LOCAL_SPRITEID = 7;
        const int LOCAL_SPRITEID_BLUEPRINT = 19;
    
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
        public BeastRidersGuild() : base(name, description, requirements, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST), LOCAL_SPRITEID, LOCAL_SPRITEID_BLUEPRINT)
        {
        }

        /// <summary>
        /// Override class to tell which card window this building uses
        /// </summary>
        /// <returns>Integer for which window type to display in the game</returns>
        protected override int GetUIType()
        {
            return UI.WindowTypes.MARKETPLACE_CARD;
        }
    }
}
