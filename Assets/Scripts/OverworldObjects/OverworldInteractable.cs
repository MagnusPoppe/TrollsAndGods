using System;
using UnityEngine;

namespace OverworldObjects
{
	public class OverworldInteractable : SpriteSystem
	{
        private Vector2 origo;

        public Vector2 Origo
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

        public OverworldInteractable( Vector2 origo, IngameObjectLibrary.Category category, int localSpriteID ) : base(localSpriteID, category)
		{
			Origo = origo;
		}

		public OverworldInteractable(IngameObjectLibrary.Category category, int localSpriteID) : base(localSpriteID, category)
        {
			
		}
	}
}
