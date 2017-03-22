namespace OverworldObjects
{
    public class GoldMine : ResourceBuilding
    {
        // BUILDING CONSTANTS FOR MAP:
        const int SHAPE = Shapes.SINGLE;
        private const int LOCAL_SPRITE_ID = 1;
        const IngameObjectLibrary.Category SPRITE_CATEGORY = IngameObjectLibrary.Category.ResourceBuildings;


        // RESOURCE VALUES:
        const Resources.type RESOURCE_ID = Resources.type.GOLD;
        private const int GOLD = 1000;
        private const int WOOD = 0;
        private const int ORE = 0;
        private const int GEM = 0;
        private const int CRYSTAL = 0;
        static Earn AMOUNT_PER_WEEK = new Earn(GOLD,WOOD,ORE,GEM,CRYSTAL);

        // USED FOR PLACEMENT OF THE OREMINE:
        const int MINIMUM_PREFERED_DISTANCE_FROM_TOWN = 10;
        const int MAXIMUM_PREFERED_DISTANCE_FROM_TOWN = 15;

        public GoldMine(Player owner)
            : base(SHAPE, owner, LOCAL_SPRITE_ID, SPRITE_CATEGORY, RESOURCE_ID, AMOUNT_PER_WEEK, MINIMUM_PREFERED_DISTANCE_FROM_TOWN, MAXIMUM_PREFERED_DISTANCE_FROM_TOWN)
        {
        }
    }
}