namespace TownView
{
    /// <summary>
    /// Placeholder class for a spesific building.
    /// Belongs to the "Unknown Town".
    /// </summary>
    public class TownHall : ResourceBuilding
    {
        // Required values for building:
        const string name = "Town Hall";
        const string description = "Build buildings here";
        static bool[] requirements = new bool[] { false, false, false, false, false, false, false, false, false, false };
        const int LOCAL_SPRITEID = 1;
        const int LOCAL_SPRITEID_BLUEPRINT = 11;

        // Resources cost: 
        const int GOLD_COST = 1500;
        const int WOOD_COST = 0;
        const int ORE_COST = 0;
        const int CRYSTAL_COST = 0;
        const int GEM_COST = 0;

        //static Earn earnings = new Earn(1000,5,5,5,5);
        static Earn earnings = new Earn(0,0,0,0,0);
        // This needs no indata since it knows its values.
        public TownHall() : base(name, description, requirements, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST), LOCAL_SPRITEID, LOCAL_SPRITEID_BLUEPRINT, earnings)
        {
            
        }

        /// <summary>
        /// Override class to tell which card window this building uses
        /// </summary>
        /// <returns>Integer for which window type to display in the game</returns>
        protected override int GetUIType()
        {
            return UI.WindowTypes.TAVERN_CARD;
        }
    }
}
