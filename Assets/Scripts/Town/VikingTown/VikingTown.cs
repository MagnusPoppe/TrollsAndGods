using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TownView
{
    /// <summary>
    /// Placeholder town. Used as a template for a spesific town.
    /// </summary>
    public class VikingTown : Town
    {
        // Sprites to be displayed in the "TOWN VEIW".
        const int spriteID = 0;
        const IngameObjectLibrary.Category category = IngameObjectLibrary.Category.Town;

        /// <summary>
        /// Default constructor:
        /// </summary>
        /// <param name="owner">Player that owns the building:</param>
        public VikingTown(Player owner) : base(owner, spriteID)
        {
            Buildings = InitializeTownBuildings();
        }


        /// <summary>
        /// Initializes all buildings that belongs to this town.
        /// </summary>
        /// <returns>Array of buildings for the building tree.</returns>
        public Building[] InitializeTownBuildings()
        {
            Building[] buildings = new Building[5];

            buildings[0] = new TownHall();
            buildings[1] = new Pallisade();
            buildings[2] = new Temple();
            buildings[3] = new DragonTower();
            buildings[4] = new TrollCave();

            return buildings;
        }

    }
}
