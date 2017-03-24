using System.Collections.Generic;
using OverworldObjects;

namespace TownView
{


    /// <summary>
    /// Town class for the players towns. Creates the tree of builings, and holds 
    /// the purchase variable that is flipped every time you buy something, and for every new day.
    /// </summary>
    public class Town : SpriteSystem
    {
        private bool hasBuiltThisRound;
        private Building[] buildings;
        private List<DwellingBuilding> relatedDwellings;
        private Hero stationedHero;
        private Hero visitingHero;
        private UnitTree stationedUnits;
        private UnitTree visitingUnits;
        Player owner;

        const IngameObjectLibrary.Category category = IngameObjectLibrary.Category.Town;
        /// <summary>
        /// Constructor that builds the buildingtree with corresponding town according to townId variable
        /// </summary>
        /// <param name="townId">which town shall be built</param>
        public Town(Player owner, int localSpriteID) :base(localSpriteID, category )
        {
            StationedUnits = new UnitTree();
            RelatedDwellings = new List<DwellingBuilding>();
            Owner = owner;
        }

        // Builds all buildings in a given town
        public void BuildAll(Town t)
        {
            for(int i = 0; i < t.buildings.Length; i++)
            {
                buildings[i].Build();
            }
        }

        // When in town window, activated by clicking on the heroes
        public void swapHeroes()
        {
            Hero tmp;
            if(StationedHero != null)
            {
                tmp = StationedHero;
                // Both is found, swap them
                if(VisitingHero != null)
                {
                    StationedHero = VisitingHero;
                    VisitingHero = tmp;
                }
                // Only stationed hero is found, move him
                else
                {
                    VisitingHero = StationedHero;
                    stationedHero = null;
                }
            }
            // Only visitingHero is found, move him
            else if (VisitingHero != null)
            {
                StationedHero = VisitingHero;
                VisitingHero = null;
            }
        }

        public bool HasBuiltThisRound
        {
            get
            {
                return hasBuiltThisRound;
            }

            set
            {
                hasBuiltThisRound = value;
            }
        }

        public Hero StationedHero
        {
            get
            {
                return stationedHero;
            }

            set
            {
                stationedHero = value;
            }
        }

        public Hero VisitingHero
        {
            get
            {
                return visitingHero;
            }

            set
            {
                visitingHero = value;
            }
        }

        public Player Owner
        {
            get
            {
                return owner;
            }

            set
            {
                owner = value;
            }
        }

        public List<DwellingBuilding> RelatedDwellings
        {
            get
            {
                return relatedDwellings;
            }

            set
            {
                relatedDwellings = value;
            }
        }

        public UnitTree VisitingUnits
        {
            get
            {
                return visitingUnits;
            }

            set
            {
                visitingUnits = value;
            }
        }

        public UnitTree StationedUnits
        {
            get
            {
                return stationedUnits;
            }

            set
            {
                stationedUnits = value;
            }
        }

        public Building[] Buildings
        {
            get
            {
                return buildings;
            }

            set
            {
                buildings = value;
            }
        }
    }

}