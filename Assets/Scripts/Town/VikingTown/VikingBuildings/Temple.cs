namespace TownView
{
    /// <summary>
    /// Temple building belonging to Viking Town
    /// </summary>
    public class Temple : Building
    {
        const string name = "Temple";
        const string description = "Eat, Pray, Program";
        static bool[] requirements = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false };
        const int LOCAL_SPRITEID = 3;
        const int LOCAL_SPRITEID_BLUEPRINT = 15;


        /// <summary>
        /// Resource cost for this building
        /// </summary>
        const int GOLD_COST = 500;
        const int WOOD_COST = 5;
        const int ORE_COST = 5;
        const int CRYSTAL_COST = 2;
        const int GEM_COST = 2;



        /// <summary>
        /// Default constructor
        /// </summary>
        public Temple() : base(name, description, requirements, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST), LOCAL_SPRITEID, LOCAL_SPRITEID_BLUEPRINT)
        {
        }

        /// <summary>
        /// Override class to tell which card window this building uses
        /// </summary>
        /// <returns>Integer for which window type to display in the game</returns>
        protected override int GetUIType()
        {
            return UI.WindowTypes.BUILDING_CARD;
        }
    }
}
