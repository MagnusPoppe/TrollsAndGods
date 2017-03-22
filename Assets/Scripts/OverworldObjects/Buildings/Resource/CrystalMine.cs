namespace OverworldObjects
{
    public class CrystalMine : ResourceBuilding
    {
        // BUILDING CONSTANTS FOR MAP:
        const int SHAPE = Shapes.SINGLE;
        private const int LOCAL_SPRITE_ID = 3;
        const IngameObjectLibrary.Category SPRITE_CATEGORY = IngameObjectLibrary.Category.ResourceBuildings;


        // RESOURCE VALUES:
        const Resources.type RESOURCE_ID = Resources.type.CRYSTAL;
        private const int GOLD = 0;
        private const int WOOD = 0;
        private const int ORE = 0;
        private const int GEM = 0;
        private const int CRYSTAL = 1;
        static Earn AMOUNT_PER_WEEK = new Earn(GOLD,WOOD,ORE,GEM,CRYSTAL);

        // USED FOR PLACEMENT OF THE OREMINE:
        const int MINIMUM_PREFERED_DISTANCE_FROM_TOWN = 15;
        const int MAXIMUM_PREFERED_DISTANCE_FROM_TOWN = 25;

        public CrystalMine(Player owner)
            : base(SHAPE, owner, LOCAL_SPRITE_ID, SPRITE_CATEGORY, RESOURCE_ID, AMOUNT_PER_WEEK, MINIMUM_PREFERED_DISTANCE_FROM_TOWN, MAXIMUM_PREFERED_DISTANCE_FROM_TOWN)
        {
        }
    }
}
