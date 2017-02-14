using System;
using UnityEngine;

namespace OverworldObjects
{
	public class OverworldInteractable
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

        public OverworldInteractable( Vector2 origo )
		{
			Origo = origo;
		}

		public OverworldInteractable()
		{
			
		}
	}
}
