using System;

namespace Filter
{
	public class MedianFilter
	{
		static int[,] DEFAULT_FILTER =
		{
			{ 1, 1, 1 },
			{ 1, 1, 1 },
			{ 1, 1, 1 }
		};

		int[,] medianFilter;
		int filterSum;

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter.MedianFilter"/> class.
        /// Uses a default filter
        /// </summary>
		public MedianFilter() : this(DEFAULT_FILTER)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter.MedianFilter"/> class.
        /// </summary>
        /// <param name="filter">Filter.</param>
		public MedianFilter(int[,] filter)
		{
			medianFilter = filter;
			filterSum = GetFilterSum(filter);
		}

        /// <summary>
        /// Gets the filter sum. The method uses addition to 
        /// add together all the values of the filter.
        /// </summary>
        /// <returns>The filter sum.</returns>
        /// <param name="filter">Filter.</param>		
        private int GetFilterSum(int[,] filter)
		{
			int sum = 0;
			for (int y = 0; y < filter.GetLength(0); y++)
				for (int x = 0; x < filter.GetLength(1); x++)
					sum += filter[x, y];

			return sum;
		}

        /// <summary>
        /// Filters a matrix with the median filter.
        /// </summary>
        /// <param name="initialX">Initial x position.</param>
        /// <param name="initialY">Initial y position.</param>
        /// <param name="matrix">Matrix to be filtered over</param>
		public int Apply(int initialX, int initialY, int[,] matrix)
		{
			int[] values = new int[filterSum];
			int range = medianFilter.GetLength(0) / 2;
			int valueindex = 0;

			int height = matrix.GetLength(0) - 1;
			int width = matrix.GetLength(1) - 1;

			for (int x = -range; x <= +range; x++)
			{
				for (int y = -range; y <= +range; y++)
				{
					if ((x + initialX >= 0 && x + initialX < width)
						&& (y + initialY >= 0 && y + initialY < height))
					{
						int runlength = medianFilter[x + range, y + range];
						for (int i = 0; i < runlength; i++)
						{
							values[valueindex++] = matrix[initialX + x, initialY + y];
						}
					}
				}
			}
			Array.Sort(values);
			return values[values.Length / 2];
		}
	}
}
