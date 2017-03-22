namespace TownView
{
    /// <summary>
    /// Placeholder class for a spesific building.
    /// Belongs to the "Unknown Town".
    /// </summary>
    public class DragonTower : Building
    {
        // Required values for building:
        const string name = "Dragon Tower";
        const string description = "Recruit dragons";
        static bool[] requirements = new bool[] { false, false, true, false, false };
        const int LOCAL_SPRITEID = 4;
        const int LOCAL_SPRITEID_BLUEPRINT = 10;


        // Resources cost: 
        const int GOLD_COST = 1000;
        const int WOOD_COST = 5;
        const int ORE_COST = 5;
        const int CRYSTAL_COST = 5;
        const int GEM_COST = 5;

        // This needs no indata since it knows its values.
        public DragonTower() : base(name, description, requirements, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST), LOCAL_SPRITEID, LOCAL_SPRITEID_BLUEPRINT)
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
