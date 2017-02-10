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
	}
}
