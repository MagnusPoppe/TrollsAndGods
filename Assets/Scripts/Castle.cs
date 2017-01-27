using System.Collections;
using UnityEngine;

namespace OverworldObjects
{
	public class Castle
	{
		Vector2 position;
		int environmentTileType;
		string name;

		public Castle(Vector2 pos)
		{
			this.position = pos;
			this.environmentTileType = 3;
			this.name = "unnamed";
		}

		public Castle( Vector2 pos, int tileType )
		{
			this.position = pos;
			this.environmentTileType = tileType;
		}

		public int GetEnvironment()
		{
			return environmentTileType;
		}

		public Vector2 GetPosition()
		{
			return position;
		}

		public override string ToString()
		{
			return "Castle " + name+ " at " +position.ToString();
		}
	}
}
