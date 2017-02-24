using System;
using UnityEngine;
using OverworldObjects;

namespace OverworldObjects
{
	public class OverworldBuilding : OverworldInteractable
	{
        private int shapeType;
        private Player player;
        private Reaction reaction;

        public int ShapeType
        {
            get
            {
                return shapeType;
            }

            set
            {
                shapeType = value;
            }
        }

        public Player Player
        {
            get
            {
                return player;
            }

            set
            {
                player = value;
            }
        }

        public Reaction Reaction
        {
            get
            {
                return reaction;
            }

            set
            {
                reaction = value;
            }
        }
        
        public OverworldBuilding(int shape, Player owner, int spriteID, IngameObjectLibrary.Category spriteCategory)
			: base(spriteCategory, spriteID)
		{
			ShapeType = shape;
			Player = owner;
		}
        public OverworldBuilding(Vector2 origo, int shape, Player owner, int spriteID, IngameObjectLibrary.Category spriteCategory)
            : base(origo, spriteCategory, spriteID)
        {
            ShapeType = shape;
            Player = owner;
        }

        public void FlipCanWalk( int[,] canWalk )
		{
			int x = (int)Origo.x;
			int y = (int)Origo.y;

			int[,] shape = Shapes.GetShape(ShapeType);

			for (int fy = 0; fy < shape.GetLength(0); fy++)
			{
				for (int fx = 0; fx < shape.GetLength(1); fx++)
				{
					int dxx = x + Shapes.dx[fx];
					int dyy = y + Shapes.dy[fy];

					if (shape[fx,fy] == 1)
						canWalk[dxx,dyy] = MapGenerator.MapMaker.CANWALK;
				}
			}
            canWalk[x,y] = MapGenerator.MapMaker.TRIGGER;
        }

        public virtual Reaction makeReaction()
        {
            //implemented in subclasses
            return null;
        } 
	}
}
