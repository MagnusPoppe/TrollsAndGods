﻿using System.Diagnostics;
using UI;

namespace TownView
{
    /// <summary>
    /// Superclass for all types of town buildings
    /// </summary>
    public class Building : SpriteSystem, Window
    {
        private string name;
        private string description;
        private bool built;
        public bool[] requirements;
        protected Cost cost;
        private Point placement;
        private int local_sprite_blueprint;
        private const IngameObjectLibrary.Category CATEGORY = IngameObjectLibrary.Category.Town;


        public bool Built
        {
            get { return built; }
        }

        public Point Placement
        {
            get { return placement; }

            set { placement = value; }
        }

        public string Name
        {
            get { return name; }

            set { name = value; }
        }

        public Cost Cost
        {
            get { return cost; }

            set { cost = value; }
        }

        public string Description
        {
            get { return description; }

            set { description = value; }
        }

        public void Build()
        {
            built = true;
        }


        /// <summary>
        /// Constructor for buildings
        /// </summary>
        /// <param name="name">Name of the given building</param>
        /// <param name="description">Description of the current building</param>
        /// <param name="requirements">The building(s) that must be built before this one is available</param>
        /// <param name="cost">The Resource cost to make the given building</param>
        /// <param name="localID">The local spriteID of the given building</param>
        /// <param name="LOCAL_SPRITEID_BLUEPRINT">The local spriteID for the blueprint of the building</param>
        public Building(string name, string description, bool[] requirements, Cost cost, int localID,
            int LOCAL_SPRITEID_BLUEPRINT) : base(localID, CATEGORY)
        {
            Name = name;
            Description = description;
            this.requirements = requirements;
            Placement = placement;
            Cost = cost;
            local_sprite_blueprint = LOCAL_SPRITEID_BLUEPRINT;
        }

        /// <summary>
        /// Inherited method UIType defers to virtual method GetUIType
        /// </summary>
        /// <returns>Integer for which window type to display in the game</returns>
        public int UIType()
        {
            return GetUIType();
        }

        /// <summary>
        /// Virtual method to be overriden by subclasses.
        /// </summary>
        /// <returns>Integer to signify that no valid card window exists for general class.</returns>
        protected virtual int GetUIType()
        {
            return WindowTypes.BUILDING_CARD;
        }

        /// <summary>
        /// Virtual method to return a buildings blueprint id
        /// </summary>
        /// <returns></returns>
        public virtual int GetSpriteBlueprintID()
        {
            return local_sprite_blueprint + IngameObjectLibrary.GetOffset(CATEGORY);
        }

        /// <summary>
        /// This function checks if a building has its requirements met
        /// </summary>
        /// <param name="town">The town the building belongs to</param>
        /// <returns>True if the building has its requirements met, false if not</returns>
        public bool MeetsRequirements(Town town)
        {
            for (int i = 0; i < town.Buildings.Length; i++)
            {
                if (requirements[i] && !town.Buildings[i].Built)
                    return false;
            }

            return true;
        }
    }
}