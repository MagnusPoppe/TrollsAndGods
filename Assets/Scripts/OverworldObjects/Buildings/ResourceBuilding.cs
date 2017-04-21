namespace OverworldObjects
{
    public class ResourceBuilding : OverworldBuilding
    {


        Resources.type resourceID;
        private Earn earnings;

        private int minDistFromTown;
        private int maxDistFromTown;

        public Resources.type ResourceID
        {
            get
            {
                return resourceID;
            }

            set
            {
                resourceID = value;
            }
        }

        public Earn Earnings
        {
            get { return earnings; }
            set { earnings = value; }
        }


        public int MinDistFromTown
        {
            get
            {
                return minDistFromTown;
            }

            set
            {
                minDistFromTown = value;
            }
        }

        public int MaxDistFromTown
        {
            get
            {
                return maxDistFromTown;
            }

            set
            {
                maxDistFromTown = value;
            }
        }

        public ResourceBuilding(int shape, Player owner, int spriteID, IngameObjectLibrary.Category spriteCategory, Resources.type resourceID, Earn amountPerWeek, int minDistFromTown, int maxDistFromTown)
            : base(shape, owner, spriteID, spriteCategory)
        {
            MinDistFromTown = minDistFromTown;
            MaxDistFromTown = maxDistFromTown;
            ResourceID = resourceID;
            Earnings = amountPerWeek;
        }

        public override Reaction makeReaction()
        {
            Reaction = new ResourceBuildingReaction(this, Origo);
            return Reaction;
        }

        public void flipReactions(Reaction[,] reactions)
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

        public override string ToString()
        {
            return base.ToString() + "\n+ " + earnings.ToString() + "/day";
        }
    }
}