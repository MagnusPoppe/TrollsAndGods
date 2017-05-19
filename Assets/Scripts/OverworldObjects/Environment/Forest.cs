namespace OverworldObjects
{
    /// <summary>
    /// Environment type Forest. Can only be placed within the "cannotWalk" areas.
    /// </summary>
    public class Forest : Environment
    {
        // An array of the available sprite IDs.
        private static int[] spriteIDs = {
            0
        };

        public int[] Sprites
        {
            get { return spriteIDs; }
        }

        private static IngameObjectLibrary.Category category = IngameObjectLibrary.Category.Environment;

        /// <summary>
        /// Creates a sprite out of a set of spriteIDs.
        /// </summary>
        public Forest() : base( spriteIDs, category)
        {

        }
    }
}