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
            // Only swap if there is a hero in on eof the spots
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

        public void SwapStationaryUnits(int unitpos1, int unitpos2)
        {
            // Merge if alike
            if (stationedUnits.GetUnits()[unitpos1] != null && stationedUnits.GetUnits()[unitpos2] != null && stationedUnits.GetUnits()[unitpos1].equals(stationedUnits.GetUnits()[unitpos2]))
            {
                int newAmount = stationedUnits.getUnitAmount(unitpos1) + stationedUnits.getUnitAmount(unitpos2);
                stationedUnits.SetUnitAmount(unitpos2, newAmount);
                stationedUnits.GetUnits()[unitpos1] = null;
            }
            // Swap
            else
            {
                int tmpAmount = stationedUnits.getUnitAmount(unitpos1);
                stationedUnits.SetUnitAmount(unitpos1, stationedUnits.getUnitAmount(unitpos2));
                stationedUnits.SetUnitAmount(unitpos2, tmpAmount);

                Unit tmp = StationedUnits.GetUnits()[unitpos1];
                stationedUnits.GetUnits()[unitpos1] = stationedUnits.GetUnits()[unitpos2];
                stationedUnits.GetUnits()[unitpos2] = tmp;
            }
        }

        public void SwapVisitingUnits(int unitpos1, int unitpos2)
        {
            // Only swap visiting if there's an hero there
            if (visitingHero != null)
            {
                // Merge if alike
                if (visitingUnits.GetUnits()[unitpos1] != null && visitingUnits.GetUnits()[unitpos2] != null && visitingUnits.GetUnits()[unitpos1].equals(visitingUnits.GetUnits()[unitpos2]))
                {
                    int newAmount = visitingUnits.getUnitAmount(unitpos1) + visitingUnits.getUnitAmount(unitpos2);
                    visitingUnits.SetUnitAmount(unitpos2, newAmount);
                    visitingUnits.GetUnits()[unitpos1] = null;
                }
                // Swap
                else
                {
                    int tmpAmount = visitingUnits.getUnitAmount(unitpos1);
                    visitingUnits.SetUnitAmount(unitpos1, visitingUnits.getUnitAmount(unitpos2));
                    visitingUnits.SetUnitAmount(unitpos2, tmpAmount);

                    Unit tmp = visitingUnits.GetUnits()[unitpos1];
                    visitingUnits.GetUnits()[unitpos1] = visitingUnits.GetUnits()[unitpos2];
                    visitingUnits.GetUnits()[unitpos2] = tmp;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitpos1">visiting position</param>
        /// <param name="unitpos2">stationed position</param>
        public void SwapVisitingStationaryUnits(int unitpos1, int unitpos2)
        {
            // Only swap visiting if there's an hero there
            if(visitingHero != null && visitingUnits.GetUnits()[unitpos1] != null)
            {
                // Merge if alike
                if (visitingUnits.GetUnits()[unitpos1] != null && stationedUnits.GetUnits()[unitpos2] != null && visitingUnits.GetUnits()[unitpos1].equals(stationedUnits.GetUnits()[unitpos2]))
                {
                    int newAmount = visitingUnits.getUnitAmount(unitpos1) + stationedUnits.getUnitAmount(unitpos2);
                    StationedUnits.SetUnitAmount(unitpos2, newAmount);
                    VisitingUnits.GetUnits()[unitpos1] = null;
                }
                // Swap
                else
                {
                    Unit tmp = visitingUnits.GetUnits()[unitpos1];
                    visitingUnits.GetUnits()[unitpos1] = stationedUnits.GetUnits()[unitpos2];
                    stationedUnits.GetUnits()[unitpos2] = tmp;


                    int tmpAmount = visitingUnits.getUnitAmount(unitpos1);
                    visitingUnits.SetUnitAmount(unitpos1, stationedUnits.getUnitAmount(unitpos2));
                    stationedUnits.SetUnitAmount(unitpos2, tmpAmount);

                    // Update herostack to the new stack (not always a stationaryhero present)
                    if(stationedHero != null)
                        stationedHero.Units = StationedUnits;
                    if (visitingHero != null)
                        visitingHero.Units = VisitingUnits;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitpos1">stationed position</param>
        /// <param name="unitpos2">visiting position</param>
        public void SwapStationedVisitingUnits(int unitpos1, int unitpos2)
        {
            // Only swap visiting if there's an hero there
            if (visitingHero != null && stationedUnits.GetUnits()[unitpos1] != null)
            {
                // Merge if alike
                if (stationedUnits.GetUnits()[unitpos1] != null && visitingUnits.GetUnits()[unitpos2] != null && stationedUnits.GetUnits()[unitpos1].equals(visitingUnits.GetUnits()[unitpos2]))
                {
                    int newAmount = visitingUnits.getUnitAmount(unitpos1) + stationedUnits.getUnitAmount(unitpos2);
                    visitingUnits.SetUnitAmount(unitpos2, newAmount);
                    stationedUnits.GetUnits()[unitpos1] = null;
                }
                // Swap
                else
                {
                    Unit tmp = StationedUnits.GetUnits()[unitpos1];
                    stationedUnits.GetUnits()[unitpos1] = visitingUnits.GetUnits()[unitpos2];
                    visitingUnits.GetUnits()[unitpos2] = tmp;


                    int tmpAmount = stationedUnits.getUnitAmount(unitpos1);
                    stationedUnits.SetUnitAmount(unitpos1, visitingUnits.getUnitAmount(unitpos2));
                    visitingUnits.SetUnitAmount(unitpos2, tmpAmount);

                    // Update herostack to the new stack
                    visitingHero.Units = VisitingUnits;
                    if(stationedHero != null)
                        stationedHero.Units = stationedUnits;
                }
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