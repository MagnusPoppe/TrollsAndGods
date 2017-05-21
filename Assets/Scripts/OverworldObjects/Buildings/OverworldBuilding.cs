using OverworldObjects;

namespace OverworldObjects
{
    /// <summary>
    /// Overworld building. This is the parent of all buildings that can be placed on the map.
    /// </summary>
	public class OverworldBuilding : OverworldInteractable
	{
        private int shapeType;
        private Player player;
        private Reaction reaction;

        /// <summary>
        /// Gets or sets the type of the shape the building has.
        /// </summary>
        /// <value>The type of the shape.</value>
        public int ShapeType
        {
            get
            {
                return shapeType;
            }

            set
            {
                shapeType = value;
            }
        }

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>The player.</value>
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

        /// <summary>
        /// Gets or sets the reaction.
        /// </summary>
        /// <value>The reaction.</value>
        public Reaction Reaction
        {
            get
            {
                return reaction;
            }

            set
            {
                reaction = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OverworldObjects.OverworldBuilding"/> class.
        /// </summary>
        /// <param name="shape">Shape.</param>
        /// <param name="owner">Owner.</param>
        /// <param name="spriteID">Sprite I.</param>
        /// <param name="spriteCategory">Sprite category.</param>
        public OverworldBuilding(int shape, Player owner, int spriteID, IngameObjectLibrary.Category spriteCategory)
			: base(spriteCategory, spriteID)
		{
			ShapeType = shape;
			Player = owner;
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="OverworldObjects.OverworldBuilding"/> class.
        /// </summary>
        /// <param name="origo">Origo.</param>
        /// <param name="shape">Shape.</param>
        /// <param name="owner">Owner.</param>
        /// <param name="spriteID">Sprite I.</param>
        /// <param name="spriteCategory">Sprite category.</param>
        public OverworldBuilding(Point origo, int shape, Player owner, int spriteID, IngameObjectLibrary.Category spriteCategory)
            : base(origo, spriteCategory, spriteID)
        {
            ShapeType = shape;
            Player = owner;
        }

        /// <summary>
        /// Flips the reactions to correspond with the building shape.
        /// </summary>
        /// <param name="reactions">Reactions.</param>
        /// <param name="hero">Hero.</param>
        public void FlipCanWalk( int[,] canWalk )
		{
			int[,] shape = Shapes.GetShape(ShapeType);

			for (int fy = 0; fy < shape.GetLength(0); fy++)
			{
				for (int fx = 0; fx < shape.GetLength(1); fx++)
				{
                    int dxx = Origo.x + Shapes.dx[fx];
                    int dyy = Origo.y + Shapes.dy[fy];

					if (shape[fx,fy] == 1)
						canWalk[dxx,dyy] = MapGenerator.MapMaker.CANNOTWALK;
				}
			}
            canWalk[Origo.x,Origo.y] = MapGenerator.MapMaker.TRIGGER;
        }

        /// <summary>
        /// Flips the reactions.
        /// </summary>
        /// <param name="reactions">Reactions.</param>
	    public virtual void flipReactions(Reaction[,] reactions)
	    {

	    }

        /// <summary>
        /// Makes the reaction.
        /// </summary>
        /// <returns>The reaction.</returns>
        public virtual Reaction makeReaction()
        {
            //implemented in subclasses
            return null;
        } 
	}
}
