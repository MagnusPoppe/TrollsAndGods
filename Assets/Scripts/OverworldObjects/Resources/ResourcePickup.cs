using JetBrains.Annotations;

namespace OverworldObjects
{
    public class ResourcePickup : SpriteSystem
    {
        protected Earn value;
        static IngameObjectLibrary.Category SPRITE_CATEGORY = IngameObjectLibrary.Category.Resources;

        public ResourcePickup(Earn value, int localSpriteID) : base(localSpriteID, SPRITE_CATEGORY)
        {}
        public ResourcePickup(int localSpriteID) : base(localSpriteID, SPRITE_CATEGORY)
        {}

        public void Pickup(Player player)
        {
            // Puts the value into the players wallet.
            value.adjustResources(player.Wallet);
        }
    }
}