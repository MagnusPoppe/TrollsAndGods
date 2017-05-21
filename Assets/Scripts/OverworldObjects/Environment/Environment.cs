using UnityEngine;

namespace OverworldObjects
{   
    /// <summary>
    /// Environment are the types of ground in the game. 
    /// The child classes that inherit from this class is spesific types of environment.
    /// 
    /// Current types of environment are:
    ///     Grass
    ///     Water
    ///     Mountain
    ///     Forest
    /// </summary>
    public class Environment : SpriteSystem
    {
        private int[] localIDs;
        private IngameObjectLibrary.Category category;
        private Environment below; // -1 means nothing to add.

        /// <summary>
        /// Initializes a new instance of the <see cref="OverworldObjects.Environment"/> class.
        /// </summary>
        /// <param name="localIDs">list of Local IDs used with sprites.</param>
        /// <param name="category">Category.</param>
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