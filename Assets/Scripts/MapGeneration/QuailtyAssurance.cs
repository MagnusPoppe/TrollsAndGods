using OverworldObjects;
using UnityEngine;

namespace MapGenerator
{
    /// <summary>
    /// Quailty assurance. This class is supposed to check if its possible to 
    /// walk and access all buildings and placed items inside the map. 
    /// </summary>
	public class QuailtyAssurance
	{

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGenerator.QuailtyAssurance"/> class.
        /// </summary>
		public QuailtyAssurance()
		{
			
		}

        /// <summary>
        /// Tests the paths between all buildings inside a given land-region.
        /// </summary>
        /// <returns><c>true</c>, if paths between buildings was accessable, <c>false</c> otherwise.</returns>
        /// <param name="region">Region.</param>
        /// <param name="canWalk">Can walk.</param>
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

        /// <summary>
        /// Tests the paths between regions.
        /// </summary>
        /// <returns><c>true</c>, if paths between regions was accessable, <c>false</c> otherwise.</returns>
        /// <param name="allRegions">All regions.</param>
        /// <param name="canWalk">Can walk.</param>
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
