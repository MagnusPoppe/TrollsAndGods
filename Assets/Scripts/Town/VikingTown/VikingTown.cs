namespace TownView
{
    /// <summary>
    /// A viking town
    /// </summary>
    public class VikingTown : Town
    {
        const int spriteID = 0;
        const IngameObjectLibrary.Category category = IngameObjectLibrary.Category.Town;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="owner">Player that owns the building:</param>
        /// <param name="position">The map position for this town</param>
        public VikingTown(Player owner, Point position) : base(owner, spriteID, position)
        {
            Buildings = InitializeTownBuildings();
        }


        /// <summary>
        /// Initializes all buildings that belongs to this town.
        /// </summary>
        /// <returns>Array of buildings for the building tree.</returns>
        public Building[] InitializeTownBuildings()
        {
            Building[] buildings = new Building[12];

            buildings[0] = new TownHall();
            buildings[1] = new Pallisade();
            buildings[2] = new Temple();
            buildings[3] = new DragonTower();
            buildings[4] = new TrollCave();
            buildings[5] = new Marketplace();
            buildings[6] = new BeastRidersGuild();
            buildings[7] = new CernianCamp();
            buildings[8] = new TrainingCamp();
            buildings[9] = new MageTower();
            buildings[10] = new WarriorsCamp();
            buildings[11] = new Workshop();
            return buildings;
        }

    }
}
