using OverworldObjects;
using UI;
using Units;
using UnityEngine;

namespace TownView
{
    public class UnitBuilding : Building, UnitPlayingCard, Dwelling
    {

        private Unit unit;
        private int unitsPresent;
        private int unitsPerWeek;

        public UnitBuilding(string name, string description, bool[] requirements, Cost cost, int localID,
            int LOCAL_SPRITEID_BLUEPRINT)
            : base(name, description, requirements, cost, localID, LOCAL_SPRITEID_BLUEPRINT)
        {
            
        }


        public int GetImage()
        {
            return Unit.GetSpriteID();
        }

        public int GetAttack()
        {
            return Unit.Unitstats.Attack;
        }

        public int GetDefense()
        {
            return Unit.Unitstats.Defence;
        }

        public int GetMagic()
        {
            return -1;
        }

        public int GetSpeed()
        {
            return Unit.Unitstats.Speed;
        }

        public int GetHealthPoints()
        {
            return Unit.Unitstats.Health;
        }

        public string GetUnitName()
        {
            return Unit.Name;
        }

        public Move[] GetMoves()
        {
            return Unit.Moves;
        }

        public Ability GetAbility()
        {
            return Unit.Ability;
        }

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

        public bool AdjustPresentUnits(int change)
        {
            
            if (UnitsPresent + change >= 0)
            {
                UnitsPresent += change;
                return true;
            }

            return false;
        }
    }
}

