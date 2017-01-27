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
			// TODO: Endre random til faktisk fungere!
			this.environmentTileType = Random.Range(3, 4);
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

		public void SetEnvironment(int environmentType)
		{
			this.environmentTileType = environmentType;
		}
	}
}
