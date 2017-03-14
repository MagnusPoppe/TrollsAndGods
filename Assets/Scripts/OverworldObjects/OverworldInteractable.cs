namespace OverworldObjects
{
	public class OverworldInteractable : SpriteSystem
	{
        private Point origo;

        public Point Origo
        {
            get
            {
                return origo;
            }

            set
            {
                origo = value;
            }
        }

        public OverworldInteractable( Point origo, IngameObjectLibrary.Category category, int localSpriteID ) : base(localSpriteID, category)
		{
			Origo = origo;
		}

		public OverworldInteractable(IngameObjectLibrary.Category category, int localSpriteID) : base(localSpriteID, category)
        {
			
		}
	}
}
