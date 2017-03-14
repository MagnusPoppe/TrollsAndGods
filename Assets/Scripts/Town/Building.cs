using UI;
namespace TownView
{


    public class Building : SpriteSystem, Window
    {
        private string name;
        private bool built;
        protected bool[] requirements;
        protected Cost cost;
        private Point placement;
        private int local_sprite_blueprint;
        private const IngameObjectLibrary.Category CATEGORY = IngameObjectLibrary.Category.Town;



        public bool Built
        {
            get
            {
                return built;
            }
        }

        public Point Placement
        {
            get
            {
                return placement;
            }

            set
            {
                placement = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public Cost Cost
        {
            get
            {
                return cost;
            }

            set
            {
                cost = value;
            }
        }

        public void Build()
        {
            built = true;
        }



        public Building(string name, bool[] requirements, Cost cost, int localID, int LOCAL_SPRITEID_BLUEPRINT) :base(localID, CATEGORY)
        {
            Name = name;
            this.requirements = requirements;
            Placement = placement;
            this.cost = cost;
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
            return WindowTypes.NO_PLAYING_CARD;
        }

        public virtual int GetSpriteBlueprintID()
        {
            return local_sprite_blueprint + IngameObjectLibrary.GetOffset(CATEGORY);
        }
    }
}