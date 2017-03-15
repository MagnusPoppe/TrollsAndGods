using UnityEngine;

namespace OverworldObjects
{
    public class Environment : SpriteSystem
    {
        private int[] localIDs;
        private static IngameObjectLibrary.Category category = IngameObjectLibrary.Category.Ground;

        public Environment( int[] localIDs ) : base(localIDs[0], category)
        {
            this.localIDs = localIDs;
        }

        /// <summary>
        /// Gets a random spriteID out of the available sprites.
        /// </summary>
        /// <returns>Valid Sprite ID</returns>
        public new int GetSpriteID()
        {
            Debug.Log("GETTING SPRITE: " +
                      localIDs[UnityEngine.Random.Range(0, localIDs.Length-1)] + IngameObjectLibrary.GetOffset(category)
            );
            return localIDs[UnityEngine.Random.Range(0, localIDs.Length-1)] + IngameObjectLibrary.GetOffset(category);
        }
    }
}