using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TownView;

namespace OverworldObjects
{

    public class Dwelling : OverworldBuilding{

        Town town;
        Player owner;
        Unit unitType;
        int unitsPresent;
        int unitsPerWeek;

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

        public Unit UnitType
        {
            get
            {
                return unitType;
            }

            set
            {
                unitType = value;
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

        public Dwelling(Vector2 origo, int shape, Player player, int spriteID, IngameObjectLibrary.Category spriteCategory, Town town, Unit unitType, int unitsPresent, int unitsPerWeek) 
            : base(origo, shape, player, spriteID, spriteCategory)
        {
            Town = town;
            UnitType = UnitType;
            UnitsPresent = unitsPresent;
            UnitsPerWeek = unitsPerWeek;
        }

        public void populate()
        {
            unitsPresent += unitsPerWeek;
        }

        public void populate(int more)
        {
            unitsPresent += unitsPerWeek * more;
        }

        public override void makeReaction(int x, int y)
        {
            Reaction = new DwellingReact(this, new Vector2(x, y));
        }
    }
}