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
        /// Constructor that builds the buildingtree with corresponding town according to townId variable
        /// </summary>
        /// <param name="townId">which town shall be built</param>
        public Town(Player owner, int localSpriteID, Point position) :base(localSpriteID, category )
        {
            Position = position;
            StationedUnits = new UnitTree();
            RelatedDwellings = new List<DwellingBuilding>();
            Owner = owner;
            StationedUnits = new UnitTree();
            VisitingUnits = new UnitTree();
        }

        // Builds all buildings in a given town
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
            Hero tmp;
            if(StationedHero != null)
            {
                tmp = StationedHero;
                // Both is found, swap them
                if(VisitingHero != null)
                {
                    StationedHero = VisitingHero;
                    VisitingHero = tmp;
                    VisitingUnits = visitingHero.Units;
                    StationedUnits = StationedHero.Units;
                }
                // Only stationed hero is found, move him
                else
                {
                    VisitingHero = StationedHero;
                    VisitingUnits = VisitingHero.Units;
                    stationedHero = null;
                    StationedUnits = null;
                }
            }
            // Only visitingHero is found, move him - but also check if you can merge him into the stationed garrison - if so, perform merge
            else if (VisitingHero != null)
            {
                if(StationedUnits == null)
                {
                    // Sets the new stationed hero and army
                    StationedHero = VisitingHero;
                    StationedUnits = StationedHero.Units;
                    VisitingHero = null;
                    VisitingUnits = null;
                }
                // TODO only swap him if theres room for his army there
                else if(stationedUnits.CanMerge(VisitingUnits))
                {
                    // Merges visitingUnits into StationedUnits
                    StationedUnits.Merge(VisitingUnits);
                    // Sets the new stationed heroes' army to the newly merged army
                    StationedHero = VisitingHero;
                    StationedHero.Units = stationedUnits;
                    VisitingHero = null;
                    VisitingUnits = null;
                }
            }
        }

        public void swapUnits(Unit unit1, Unit Unit2)
        {
            //TODO cannot swap if one of them is visitingunit without visitinghero

            //TODO merge unit2 into unit1 if the same


            //TODO swap unit2 and unit1
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