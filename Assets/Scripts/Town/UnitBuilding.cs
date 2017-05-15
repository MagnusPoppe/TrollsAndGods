using OverworldObjects;
using UI;
using Units;
using UnityEngine;

namespace TownView
{

    /// <summary>
    /// Superclass for Town buildings that sell and breed units
    /// </summary>
    public class UnitBuilding : Building, UnitPlayingCard, Dwelling
    {

        private Unit unit;
        private int unitsPresent;
        private int unitsPerWeek;

        /// <summary>
        /// Constructor for a unit building
        /// </summary>
        /// <param name="name">Name of the given building</param>
        /// <param name="description">Description of the current building</param>
        /// <param name="requirements">The building(s) that must be built before this one is available</param>
        /// <param name="cost">The Resource cost to make the given building</param>
        /// <param name="localID">The local spriteID of the given building</param>
        /// <param name="LOCAL_SPRITEID_BLUEPRINT">The local spriteID for the blueprint of the building</param>
        public UnitBuilding(string name, string description, bool[] requirements, Cost cost, int localID,
            int LOCAL_SPRITEID_BLUEPRINT)
            : base(name, description, requirements, cost, localID, LOCAL_SPRITEID_BLUEPRINT)
        {
            
        }

        /// <summary>
        /// Gets the spriteID for the unit this building produces
        /// </summary>
        /// <returns>Local spriteID for this building's unit</returns>
        public int GetImage()
        {
            return Unit.GetSpriteID();
        }

        /// <summary>
        /// Gets the attack for the unit this building produces
        /// </summary>
        /// <returns>The attack value for this building's unit</returns>
        public int GetAttack()
        {
            return Unit.Unitstats.Attack;
        }

        /// <summary>
        /// Gets the defense for the unit this building produces
        /// </summary>
        /// <returns>The defense value for this building's unit</returns>
        public int GetDefense()
        {
            return Unit.Unitstats.Defence;
        }

        /// <summary>
        /// Gets the magic value for the unit this building produces
        /// </summary>
        /// <returns>The magic value for this building's unit</returns>
        public int GetMagic()
        {
            return -1;
        }

        /// <summary>
        /// Gets the speed value for the unit this building produces
        /// </summary>
        /// <returns>The speed value for this building's unit</returns>
        public int GetSpeed()
        {
            return Unit.Unitstats.Speed;
        }

        /// <summary>
        /// Gets the health point value for the unit this building produces
        /// </summary>
        /// <returns>The health point value for this building's unit</returns>
        public int GetHealthPoints()
        {
            return Unit.Unitstats.Health;
        }

        /// <summary>
        /// Gets the name for the unit this building produces
        /// </summary>
        /// <returns>The name for this building's unit</returns>
        public string GetUnitName()
        {
            return Unit.Name;
        }

        /// <summary>
        /// Gets the list of moves for the unit this building produces
        /// </summary>
        /// <returns>The list of moves for this building's unit</returns>
        public Move[] GetMoves()
        {
            return Unit.Moves;
        }

        /// <summary>
        /// Gets the ability for the unit this building produces
        /// </summary>
        /// <returns>The ability for this building's unit</returns>
        public Ability GetAbility()
        {
            return Unit.Ability;
        }


        /// <summary>
        /// Getter and setter for this building's unit
        /// </summary>
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

        /// <summary>
        /// Getter and setter for how many units are present to be bought
        /// </summary>
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

        // Getter and setter for how many units are made each week by this building
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

        /// <summary>
        /// This function adjusts how many units are present in a given instance of this building
        /// </summary>
        /// <param name="change">How many units to add or remove</param>
        /// <returns>Returns true if the unit change succeeded, false if not.</returns>
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

