using JetBrains.Annotations;

namespace OverworldObjects
{
    /// <summary>
    /// Resource pickups are resources placed on the map for a 
    /// one time bonus to a given resource.
    /// </summary>
    public class ResourcePickup : SpriteSystem
    {
        protected Earn value;
        static IngameObjectLibrary.Category SPRITE_CATEGORY = IngameObjectLibrary.Category.Resources;

        public ResourcePickup(Earn value, int localSpriteID) : base(localSpriteID, SPRITE_CATEGORY)
        {}
        public ResourcePickup(int localSpriteID) : base(localSpriteID, SPRITE_CATEGORY)
        {}

        /// <summary>
        /// Pickup by specified player.
        /// </summary>
        /// <param name="player">Player.</param>
        public void Pickup(Player player)
        {
            // Puts the value into the players wallet.
            value.adjustResources(player.Wallet);
        }
    }
}