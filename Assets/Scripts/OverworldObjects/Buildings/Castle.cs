using TownView;

namespace OverworldObjects
{
    /// <summary>
    /// Generic clastle class.
    /// </summary>
	public class Castle : OverworldBuilding
	{
		Environment environment;

        /// <summary>
        /// Gets the environment that surrounds the castle.
        /// </summary>
        /// <value>The environment.</value>
	    public Environment Environment
	    {
	        get { return environment; }
	    }

	    string name;
        Town town;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverworldObjects.Castle"/> class.
        /// </summary>
        /// <param name="origo">Origo.</param>
        /// <param name="shape">Shape.</param>
        /// <param name="owner">Owner.</param>
        /// <param name="spriteID">Sprite I.</param>
        /// <param name="name">Name.</param>
        /// <param name="spriteCategory">Sprite category.</param>
        /// <param name="environment">Environment.</param>
        public Castle(Point origo, int shape, Player owner, int spriteID, string name, IngameObjectLibrary.Category spriteCategory, Environment environment)
            : base(origo, shape, owner, spriteID, spriteCategory)
        {
            Name = name;
            this.environment = environment;
            Town = town;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OverworldObjects.Castle"/> class.
        /// </summary>
        /// <param name="shape">Shape.</param>
        /// <param name="owner">Owner.</param>
        /// <param name="spriteID">Sprite I.</param>
        /// <param name="spriteCategory">Sprite category.</param>
        public Castle( int shape, Player owner, int spriteID, IngameObjectLibrary.Category spriteCategory)
                : base( shape, owner, spriteID, spriteCategory)
        {
            Name = name;
            Town = town;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
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

        /// <summary>
        /// Gets or sets the town.
        /// </summary>
        /// <value>The town.</value>
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

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <returns>The position.</returns>
        public Point GetPosition()
		{
			return Origo;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="OverworldObjects.Castle"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="OverworldObjects.Castle"/>.</returns>
        public override string ToString()
		{
			return "Castle " + Name+ " at " +Origo.ToString();
		}

        /// <summary>
        /// Makes the reaction.
        /// </summary>
        /// <returns>The reaction.</returns>
        public override Reaction makeReaction()
        {
            return Reaction = new CastleReact(this, Origo);
        }

        /// <summary>
        /// Flips the reactions to correspond with the castle shape.
        /// </summary>
        /// <param name="reactions">Reactions.</param>
        /// <param name="hero">Hero.</param>
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


	    /// <summary>
	    /// Changes owner of castle to Player whose turn it is
	    /// </summary>
	    /// <param name="cr">CastleReact</param>
	    public void changeCastleOwner(CastleReact cr, Player player, Hero hero)
	    {
	        cr.Castle.Player.Castle.Remove(cr.Castle);
	        cr.Castle.Player = player;
	        cr.Castle.Town.Owner = player;
	        cr.Castle.Town.VisitingHero = hero;
	        player.Castle.Add(cr.Castle);
	    }
    }
}
