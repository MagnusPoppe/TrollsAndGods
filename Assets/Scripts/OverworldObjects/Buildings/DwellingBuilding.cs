﻿using TownView;

namespace OverworldObjects
{

    public class DwellingBuilding : OverworldBuilding, Dwelling {

        Town town;
        Player owner;
        private Unit unit;
        private int unitsPresent;
        private int unitsPerWeek;


        public Unit Unit
        {
            get
            {
                return unit;
            }
            set
            {
                unit = value;
            }
        }

        public int UnitsPresent
        {
            get
            {
                return unitsPresent;
            }
            set
            {
                unitsPresent = value;
            }
        }

        public int UnitsPerWeek
        {
            get
            {
                return unitsPerWeek;
            }
            set
            {
                unitsPerWeek = value;
            }
        }

        public Town Town
        {
            get
            {
                return town;
            }

            set
            {
                town = value;
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

        public DwellingBuilding(Point origo, int shape, Player player, int spriteID, IngameObjectLibrary.Category spriteCategory, Town town, Unit unitType, int unitsPresent, int unitsPerWeek) 
            : base(origo, shape, player, spriteID, spriteCategory)
        {
            Town = town;
            Unit = unit;
            UnitsPresent = unitsPresent;
            UnitsPerWeek = unitsPerWeek;
        }

        public void Populate()
        {
            unitsPresent += unitsPerWeek;
        }

        public void Populate(int more)
        {
            unitsPresent += unitsPerWeek * more;
        }

        public override Reaction makeReaction()
        {
            return Reaction = new DwellingReact(this, Origo);
        }


    }
}