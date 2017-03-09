using System;
using OverworldObjects;
using UnityEngine;

namespace MapGenerator
{
	public class QuailtyAssurance
	{
		public QuailtyAssurance()
		{
			
		}

		public bool TestPathsBetweenBuildings(LandRegion region, int[,] canWalk)
		{
			AStarAlgo aStar = new AStarAlgo(canWalk, canWalk.GetLength(0), canWalk.GetLength(1), false);

			Point castlePlacement = region.GetCastle().GetPosition();
			foreach (OverworldBuilding building in region.GetBuildings())
			{
                Point buildingPlacement = building.Origo;
				if (aStar.calculate(castlePlacement, buildingPlacement).Count == 0)
				{
					return false;
				}
			}

			return true;
		}

		public bool TestPathsBetweenRegions(LandRegion[] allRegions, int[,] canWalk)
		{
			AStarAlgo aStar = new AStarAlgo(canWalk, canWalk.GetLength(0), canWalk.GetLength(1), false);

			for (int i = 0; i <= allRegions.GetLength(0); i++)
			{
				if (TestPathsBetweenBuildings(allRegions[i], canWalk))
				{
                    Point a = allRegions[i - 1].GetCastle().GetPosition();
                    Point b;
					if (i == 0)
					{
						b = allRegions[allRegions.GetLength(0)-1].GetCastle().GetPosition();
					}
					else
					{
						b = allRegions[i].GetCastle().GetPosition();
					}

					if (aStar.calculate(a, b).Count == 0)
						return false;
				}
				else 
				{
					Debug.Log("Cannot Walk between all buildings in region " + allRegions[i].ToString());
					return false;
				}
			}
			return true;
		}
	}
}
