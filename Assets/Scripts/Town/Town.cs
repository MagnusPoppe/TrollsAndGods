using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OverworldObjects;

namespace TownView
{


    /// <summary>
    /// Town class for the players towns. Creates the tree of builings, and holds 
    /// the purchase variable that is flipped every time you buy something, and for every new day.
    /// </summary>
    public class Town
    {
        private bool hasBuiltThisRound;
        private Building[] buildings;
        private List<Dwelling> relatedDwellings;
        private Hero stationedHero;
        private Hero visitingHero;
        private UnitTree stationedUnits;
        private UnitTree visitingUnits;
        Player owner;

        /// <summary>
        /// Constructor that builds the buildingtree with corresponding town according to townId variable
        /// </summary>
        /// <param name="townId">which town shall be built</param>
        public Town(Player owner)
        {
            StationedUnits = new UnitTree();
            RelatedDwellings = new List<Dwelling>();
            Owner = owner;
        }

        //public bool canBuild(Building b)
        //{
        //    for (int i = 0; i < Buildings.Length; i++)
        //    {
        //        if (b.GetRequirements()[i] && !Buildings[i].IsBuilt())
        //            return false;
        //    }
        //    return true;
        //}

        //public void Build(Building b)
        //{
        //    b.SetBuilt(true);
        //    hasBuiltThisRound = true;
        //}

        //public void updateAvailableCreatures()
        //{
        //    foreach (Building b in Buildings)
        //    {
        //        if (b.IsBuilt() && b.ProducesCreatures)
        //        {
        //            int extra = 0;
        //            foreach (Dwelling d in relatedDwellings)
        //            {
        //                if (d.UnitType.equals(b.ProducedUnit) && d.Owner.equals(owner))
        //                {
        //                    extra += d.UnitsPerWeek;
        //                }
        //            }
        //            b.populate(extra);
        //        }
        //    }
        //}

        //public void updateDwellingOwnerChange(Dwelling d)
        //{
        //    foreach (Building b in Buildings)
        //    {
        //        if (b.IsBuilt() && b.ProducesCreatures && b.ProducedUnit.equals(d.UnitType))
        //        {
        //            if (d.Owner.equals(owner))
        //            {
        //                b.AmountOfCreatures += d.UnitsPresent;
        //            }
        //            else
        //            {
        //                b.AmountOfCreatures -= d.UnitsPresent;
        //            }
        //        }
        //    }
        //}

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

        protected Hero StationedHero
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

        protected Hero VisitingHero
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

        public List<Dwelling> RelatedDwellings
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