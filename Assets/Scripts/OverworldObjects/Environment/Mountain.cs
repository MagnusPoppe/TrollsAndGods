namespace OverworldObjects
{
    /// <summary>
    /// Environment type Mountain. Contains the shape of the mountain so that
    /// it can be placed within a confined space.
    /// </summary>
    public class Mountain : Environment
    {
        // An array of the available sprite IDs.
        private static int[] spriteIDs = {
            1, 2, 3, 4, 5
        };
        private static IngameObjectLibrary.Category category = IngameObjectLibrary.Category.Environment;

        private int[,] shape;
        public int[,] Shape
        {
            get { return shape; }
        }

        public int[] Sprites
        {
            get { return spriteIDs; }
        }

        private int below = 6;

        /// <summary>
        /// Gets the ground type below.
        /// </summary>
        /// <value>The below.</value>
        public int Below
        {
            get { return IngameObjectLibrary.GetOffset(category) + below; }
        }


        /// <summary>
        /// Creates a sprite out of a set of spriteIDs.
        /// </summary>
        public Mountain() : base( spriteIDs , category)
        {
            shape =  new int[,] // TRIPLE x3 LEFT SPECIAL VERSION.
            {
                // ORIGO = 1, OTHER SPACE = 2, ELSE = UNTOUCHED.
                { 0, 0, 0, 2, 0 },
                { 0, 0, 2, 2, 0 },
                { 0, 0, 2, 2, 2 },
                { 0, 0, 2, 2, 0 },
                { 0, 0, 0, 1, 0 }
            };
        }
    }
}