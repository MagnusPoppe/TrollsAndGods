namespace OverworldObjects
{
    public class Grass : Environment
    {
        // An array of the available sprite IDs.
        private static int[] spriteIDs = {
            13,14,15,16
        };

        /// <summary>
        /// Creates a sprite out of a set of spriteIDs.
        /// </summary>
        public Grass() : base(spriteIDs)
        {

        }
    }
}