using System;
using UnityEngine;

namespace OverworldObjects
{
	public class OverworldInteractable
	{
		protected Vector2 origo;

		public OverworldInteractable( Vector2 center )
		{
			this.origo = center;
		}

		public OverworldInteractable()
		{
			
		}

		public Vector2 GetOrigo()
		{
			return origo;
		}

		public void SetOrigo(Vector2 origo)
		{
			this.origo = origo;
		}
	}
}
