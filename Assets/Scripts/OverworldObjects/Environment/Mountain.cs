namespace OverworldObjects
{
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