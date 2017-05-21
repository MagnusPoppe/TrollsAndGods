namespace OverworldObjects
{
    /// <summary>
    /// Resource building. Parent of all buildings that generate resources.
    /// </summary>
    public class ResourceBuilding : OverworldBuilding
    {


        Resources.type resourceID;
        private Earn earnings;

        private int minDistFromTown;
        private int maxDistFromTown;

        /// <summary>
        /// Gets or sets the resource Type.
        /// </summary>
        /// <value>The resource ID.</value>
        public Resources.type ResourceID
        {
            get {return resourceID;}
            set {resourceID = value;}
        }

        /// <summary>
        /// Gets or sets the earnings.
        /// </summary>
        /// <value>The earnings.</value>
        public Earn Earnings
        {
            get { return earnings; }
            set { earnings = value; }
        }


        /// <summary>
        /// Gets or sets the minimum distance from town.
        /// </summary>
        /// <value>The minimum distance from town.</value>
        public int MinDistFromTown
        {
            get { return minDistFromTown;}
            set { minDistFromTown = value;}
        }

        /// <summary>
        /// Gets or sets the max distance from town.
        /// </summary>
        /// <value>The max distance from town.</value>
        public int MaxDistFromTown
        {
            get { return maxDistFromTown; }

            set { maxDistFromTown = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OverworldObjects.ResourceBuilding"/> class.
        /// </summary>
        /// <param name="shape">Shape.</param>
        /// <param name="owner">Owner.</param>
        /// <param name="spriteID">Sprite I.</param>
        /// <param name="spriteCategory">Sprite category.</param>
        /// <param name="resourceID">Resource I.</param>
        /// <param name="amountPerWeek">Amount per week.</param>
        /// <param name="minDistFromTown">Minimum dist from town.</param>
        /// <param name="maxDistFromTown">Max dist from town.</param>
        public ResourceBuilding(int shape, Player owner, int spriteID, IngameObjectLibrary.Category spriteCategory, Resources.type resourceID, Earn amountPerWeek, int minDistFromTown, int maxDistFromTown)
            : base(shape, owner, spriteID, spriteCategory)
        {
            MinDistFromTown = minDistFromTown;
            MaxDistFromTown = maxDistFromTown;
            ResourceID = resourceID;
            Earnings = amountPerWeek;
        }

        /// <summary>
        /// Makes the reaction.
        /// </summary>
        /// <returns>The reaction.</returns>
        public override Reaction makeReaction()
        {
            Reaction = new ResourceBuildingReaction(this, Origo);
            return Reaction;
        }

        /// <summary>
        /// Flips the reactions to correspond with the building shape.
        /// </summary>
        /// <param name="reactions">Reactions.</param>
        /// <param name="hero">Hero.</param>
        public override void flipReactions(Reaction[,] reactions)
        {
            int x = (int)Origo.x;
            int y = (int)Origo.y;

            int[,] shape = Shapes.GetShape(ShapeType);

            for (int fy = 0; fy < shape.GetLength(0); fy++)
            {
                for (int fx = 0; fx < shape.GetLength(1); fx++)
                {
                    int dxx = x + Shapes.dx[fx];
                    int dyy = y + Shapes.dy[fy];

                    if (shape[fx, fy] == 1)
                    {
                        reactions[dxx, dyy] = new ResourceBuildingReaction(this, new Point(dxx, dyy));
                    }
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="OverworldObjects.ResourceBuilding"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="OverworldObjects.ResourceBuilding"/>.</returns>
        public override string ToString()
        {
            return base.ToString() + "\n+ " + earnings.ToString() + "/day";
        }
    }
}