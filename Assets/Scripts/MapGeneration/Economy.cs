using System;
using UnityEngine;
namespace MapGenerator
{
	public class Economy
	{
		public const int SLUMS 		= 0;
		public const int POOR 		= 1;
		public const int RICH 		= 2;
		public const int ABUNDANT 	= 3;

		public int goldMineCount;
		public int woodMineCount;
		public int oreMineCount;
		public int crystalMineCount;
		public int gemMineCount;
		public int sulfurMineCount;
		public int mercuryMineCount;

		public int totalBuildingCount;

		/// <summary>
		/// Sets the economy for a given region based on 
		/// one of the constants in the class.
		/// 
		/// Initializes a new instance of the <see cref="T:MapGenerator.Economy"/> class.
		/// </summary>
		/// <param name="type">Type of constant, Range = 0 -> 3.</param>
		public Economy( int type )
		{
			if (type == SLUMS)
			{
				goldMineCount 		= 0;
				woodMineCount 		= 1;
				oreMineCount 		= 1;
				crystalMineCount 	= 0;
				gemMineCount 		= 0;
				sulfurMineCount 	= 0;
				mercuryMineCount 	= 0;
			}
			else if (type == POOR)
			{
				goldMineCount 		= 1;
				woodMineCount 		= 1;
				oreMineCount 		= 1;
				crystalMineCount 	= 0;
				gemMineCount 		= 0;
				sulfurMineCount 	= 0;
				mercuryMineCount 	= 0;

				// TODO: CHOOSE RESOURCE OTHER THAN CASTLE MAIN RESOURCE

				int random = UnityEngine.Random.Range(0, 3);
				if (random == 0) crystalMineCount   = 1;
				if (random == 1) gemMineCount		= 1;
				if (random == 2) sulfurMineCount 	= 1;
				if (random == 3) mercuryMineCount 	= 1;	
			}
			else if (type == RICH)
			{
				goldMineCount 		= 1;
				woodMineCount 		= 1;
				oreMineCount 		= 1;
				crystalMineCount 	= 1;
				gemMineCount 		= 1;
				sulfurMineCount 	= 1;
				mercuryMineCount 	= 1;
			}
			else if (type == ABUNDANT)
			{
				goldMineCount 		= 2;
				woodMineCount 		= 2;
				oreMineCount 		= 2;
				crystalMineCount 	= 1;
				gemMineCount 		= 1;
				sulfurMineCount 	= 1;
				mercuryMineCount 	= 1;
			}
			else
			{
				// TODO THROW EXCEPTION
			}

			totalBuildingCount =
				goldMineCount +
				woodMineCount +
				oreMineCount +
				crystalMineCount +
				gemMineCount +
				sulfurMineCount +
				mercuryMineCount;
		}

	}
}
