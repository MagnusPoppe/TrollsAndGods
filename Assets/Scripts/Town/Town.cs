using System.Collections.Generic;
using OverworldObjects;
using UnityEngine;

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
        private Point position;
        Player owner;

        const IngameObjectLibrary.Category category = IngameObjectLibrary.Category.Town;


        /// <summary>
        /// Consturctor for towns
        /// </summary>
        /// <param name="owner">The player that owns the town, NULL if not owned</param>
        /// <param name="localSpriteID">The spriteID for the given town</param>
        /// <param name="position">The map position for the town</param>
        public Town(Player owner, int localSpriteID, Point position) :base(localSpriteID, category )
        {
            Position = position;
            StationedUnits = new UnitTree();
            RelatedDwellings = new List<DwellingBuilding>();
            Owner = owner;
            StationedUnits = new UnitTree();
            VisitingUnits = new UnitTree();
        }

        /// <summary>
        /// Debug method to build all buildings in a town
        /// </summary>
        /// <param name="t">The town which should have its buildings built</param>
        public void BuildAll(Town t)
        {
            for(int i = 0; i < t.buildings.Length; i++)
            {
                buildings[i].Build();
            }
        }

        // When in town window, activated by clicking on first and then second hero
        public void swapHeroes()
        {
            // Only swap if there is a hero in one of the spots
            if (visitingHero != null || stationedHero != null)
            {
                // TODO check merge, and merge - if there's not an hero in stationedarmy
                if (stationedHero == null)
                {
                    if (VisitingHero.Units.CanMerge(stationedUnits))
                    {
                        VisitingHero.Units.Merge(stationedUnits);
                    }
                }

                // Swap heroes
                Hero tmpHero = visitingHero;
                visitingHero = stationedHero;
                stationedHero = tmpHero;

                // Swap town's armies to the new heroes armies
                if (visitingHero == null)
                    visitingUnits = new UnitTree();
                else
                    visitingUnits = visitingHero.Units;
                if (StationedHero == null)
                    stationedUnits = new UnitTree();
                else
                    stationedUnits = stationedHero.Units;
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

        public Point Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }
    }

}