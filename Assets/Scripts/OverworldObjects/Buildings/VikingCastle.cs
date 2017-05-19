using OverworldObjects;
using TownView;

namespace OverworldObjects
{

    /// <summary>
    /// The viking castle is the castle placed on top of a vikingship grave.
    /// </summary>
    class VikingCastle : Castle
    {
        // Shape of the sprite:
        const int shape = Shapes.TRIPLEx3_LEFT;
        // Name
        const string NAME = "Vikingtown";

        // Unique sprite ID for this castle to be displayed in the overworld map: 
        private const int LOCAL_SPRITE_ID = 1;
        const IngameObjectLibrary.Category SPRITE_CATEGORY = IngameObjectLibrary.Category.Castle;

        // The enviroment that surrounds the castle:
        private static Environment environment = new Grass();

        /// <summary>
        /// Default constructor:
        /// Sets all the needed values to be a castle. Also creates the town that belongs to
        /// the castle.
        /// </summary>
        /// <param name="origo">pkt the castle should be placed at.</param>
        /// <param name="owner">Player that owns the castle.</param>
        public VikingCastle(Point origo, Player owner)
            : base(origo, shape, owner, LOCAL_SPRITE_ID, NAME, SPRITE_CATEGORY, environment)
        {
            Town = new VikingTown(owner, origo);

            // TODO: Fjern temp som bygger alle bygninger
            //Town.BuildAll(Town);

            // Builds Town Hall-type building that can build other buildings
            Town.Buildings[0].Build();
        }

        /*
        /// <summary>
        /// Sets all the needed values to be a castle. Also creates the town that belongs to
        /// the castle. This constructor allows the origopkt to be placed after the creation of the town.
        /// </summary>
        /// <param name="owner">Player that owns the castle.</param>
        public VikingCastle(Player owner)
            : base(shape, owner, LOCAL_SPRITE_ID, SPRITE_CATEGORY)
        {
            Town = new VikingTown(owner);

            // Builds Town Hall-type building that can build other buildings
            Town.Buildings[0].Build();
        }*/
    }
}
