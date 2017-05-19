using TownView;

namespace OverworldObjects
{
    /// <summary>
    /// Dwelling building. A building, but also a dwelling?! YES.
    /// </summary>
    public class DwellingBuilding : OverworldBuilding, Dwelling {

        Town town;
        Player owner;
        private Unit unit;
        private int unitsPresent;
        private int unitsPerWeek;

        /// <summary>
        /// Gets or sets the unit that can be purchased in the dwelling.
        /// </summary>
        /// <value>The unit.</value>
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
        /// Gets or sets the units present. This is how many units
        /// you can buy actually.
        /// </summary>
        /// <value>The units present.</value>
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

        /// <summary>
        /// Gets or sets the units per week or growth rate.
        /// </summary>
        /// <value>The units per week.</value>
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
        /// Gets or sets the town the building is linked to.
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
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="OverworldObjects.DwellingBuilding"/> class.
        /// </summary>
        /// <param name="origo">Origo.</param>
        /// <param name="shape">Shape.</param>
        /// <param name="player">Player.</param>
        /// <param name="spriteID">Sprite I.</param>
        /// <param name="spriteCategory">Sprite category.</param>
        /// <param name="town">Town.</param>
        /// <param name="unitType">Unit type.</param>
        /// <param name="unitsPresent">Units present.</param>
        /// <param name="unitsPerWeek">Units per week.</param>
        public DwellingBuilding(Point origo, int shape, Player player, int spriteID, IngameObjectLibrary.Category spriteCategory, Town town, Unit unitType, int unitsPresent, int unitsPerWeek) 
            : base(origo, shape, player, spriteID, spriteCategory)
        {
            Town = town;
            Unit = unit;
            UnitsPresent = unitsPresent;
            UnitsPerWeek = unitsPerWeek;
        }

        /// <summary>
        /// Populate this dwelling.
        /// </summary>
        public void Populate()
        {
            unitsPresent += unitsPerWeek;
        }

        /// <summary>
        /// Populate the dwelling with specified number * units per week..
        /// </summary>
        /// <param name="more">More.</param>
        public void Populate(int more)
        {
            unitsPresent += unitsPerWeek * more;
        }

        /// <summary>
        /// Makes the reaction.
        /// </summary>
        /// <returns>The reaction.</returns>
        public override Reaction makeReaction()
        {
            return Reaction = new DwellingReact(this, Origo);
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
                        reactions[dxx, dyy] = new DwellingReact(this, new Point(dxx, dyy));
                    }
                }
            }
        }
        public override string ToString()
        {
            return base.ToString() + "\nRecruit " + unitsPresent + "/" + unitsPerWeek + unit.Name;
        }
    }
}