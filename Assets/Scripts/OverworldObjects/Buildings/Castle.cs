﻿using System.Collections;
using UnityEngine;

namespace Buildings
{
	public class Castle : OverworldBuilding
	{
		int environmentTileType;
		string name;

        public Castle(Vector2 origo, int shape, int owner, int spriteID, int environmentTileType)
            : base(origo, shape, owner, spriteID)
        {
            Name = "unnamed";
            EnvironmentTileType = environmentTileType;
        }
        public Castle( int shape, int owner, int spriteID)
                : base( shape, owner, spriteID)
        {
            this.Name = "unnamed";
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
	}
}
