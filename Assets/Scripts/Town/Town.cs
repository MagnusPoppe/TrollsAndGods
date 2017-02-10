using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Town
{
    /// <summary>
    /// Town class for the players towns. Creates the tree of builings, and holds 
    /// the purchase variable that is flipped every time you buy something, and for every new day.
    /// </summary>
    public class Town
    {
        private bool hasBuiltThisRound;
        private Building[] buildings;
        private Hero stationedHero;
        private Hero visitingHero;
        private UnitTree unitTree;
        Player player;

        /// <summary>
        /// Constructor that builds the buildingtree with corresponding town according to townId variable
        /// </summary>
        /// <param name="townId">which town shall be built</param>
        public Town(int townId, Building[] buildings, Player player)
        {
            this.buildings = buildings;
            unitTree = new UnitTree();
            Player = player;
        }
        public bool canBuild(Building b)
        {
            for (int i = 0; i < buildings.Length; i++)
            {
                if (b.GetRequirements()[i] && !buildings[i].IsBuilt())
                    return false;
            }
            return true;
        }

        public void Build(Building b)
        {
            b.SetBuilt(true);
            hasBuiltThisRound = true;
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

        public Player Player
        {
            get
            {
                return player;
            }

            set
            {
                player = value;
            }
        }
    }
}