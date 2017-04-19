using UnityEngine;

namespace OverworldObjects
{
    public class Environment : SpriteSystem
    {
        private int[] localIDs;
        private IngameObjectLibrary.Category category;
        private Environment below; // -1 means nothing to add.

        public Environment( int[] localIDs, IngameObjectLibrary.Category category) : base(localIDs[0], category)
        {
            this.localIDs = localIDs;
            this.category = category;
            this.below = below;
        }

        /// <summary>
        /// Gets a random spriteID out of the available sprites.
        /// </summary>
        /// <returns>Valid Sprite ID</returns>
        public new int GetSpriteID()
        {
            return localIDs[UnityEngine.Random.Range(0, localIDs.Length-1)] + IngameObjectLibrary.GetOffset(category);
        }

        /// <summary>
        /// Gets what environment type should be below the given game object.
        /// this is not shown, but is used in placement algoritms.
        /// </summary>
        /// <returns> what sprite should be below the game object.</returns>
        public Environment GetBelow()
        {
            return below;
        }
    }
}