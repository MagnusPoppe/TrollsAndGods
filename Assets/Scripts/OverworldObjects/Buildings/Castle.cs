using TownView;

namespace OverworldObjects
{
	public class Castle : OverworldBuilding
	{
		Environment environment;

	    public Environment Environment
	    {
	        get { return environment; }
	    }

	    string name;
        Town town;

        public Castle(Point origo, int shape, Player owner, int spriteID, string name, IngameObjectLibrary.Category spriteCategory, Environment environment)
            : base(origo, shape, owner, spriteID, spriteCategory)
        {
            Name = name;
            this.environment = environment;
            Town = town;
        }
        public Castle( int shape, Player owner, int spriteID, IngameObjectLibrary.Category spriteCategory)
                : base( shape, owner, spriteID, spriteCategory)
        {
            Name = name;
            Town = town;
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

        public Point GetPosition()
		{
			return Origo;
        }

        public override string ToString()
		{
			return "Castle " + Name+ " at " +Origo.ToString();
		}

        public override Reaction makeReaction()
        {
            return Reaction = new CastleReact(this, Origo);
        }

        public void flipReactions(Reaction[,] reactions, Hero hero)
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
                        reactions[dxx, dyy] = new CastleReact(this, new Point(dxx, dyy));
                    }
                }
            }
            if(hero != null)
            {
                CastleReact react = (CastleReact)reactions[x, y];
                react.PreReaction = new HeroMeetReact(hero, new Point(x, y));
            }
        }
    }
}
