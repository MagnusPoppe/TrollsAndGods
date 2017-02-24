using System.Collections;
using UnityEngine;
using TownView;

namespace OverworldObjects
{
	public class Castle : OverworldBuilding
	{
		int environmentTileType;
		string name;
        Town town;

        public Castle(Vector2 origo, int shape, Player owner, int spriteID, IngameObjectLibrary.Category spriteCategory, int environmentTileType)
            : base(origo, shape, owner, spriteID, spriteCategory)
        {
            Name = "unnamed";
            EnvironmentTileType = environmentTileType;
            Town = town;
        }
        public Castle( int shape, Player owner, int spriteID, IngameObjectLibrary.Category spriteCategory)
                : base( shape, owner, spriteID, spriteCategory)
        {
            this.Name = "unnamed";
            Town = town;
        }

        public int EnvironmentTileType
        {
            get
            {
                return environmentTileType;
            }

            set
            {
                environmentTileType = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public Town Town
        {
            get
            {
                return town;
            }

            set
            {
                town = value;
            }
        }

        public int GetEnvironment()
		{
			return EnvironmentTileType;
		}

		public Vector2 GetPosition()
		{
			return Origo;
        }

        public void SetEnvironment(int environmentType)
        {
            this.EnvironmentTileType = environmentType;
        }

        public override string ToString()
		{
			return "Castle " + Name+ " at " +Origo.ToString();
		}

        public override Reaction makeReaction()
        {
            return Reaction = new CastleReact(this, Origo);
        }
	}
}
