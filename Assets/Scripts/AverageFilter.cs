using System;

public class AverageFilter
{

    static int[, ] DEFAULT_FILTER =
    {
        { 1, 1, 1 },
        { 1, 1, 1 },
        { 1, 1, 1 }
    };

    int[,] avgFilter;
    int filterSum;

    /// <summary>
    /// Default contstructor. Uses the basic average filter.
    /// </summary>
    public AverageFilter() : this( DEFAULT_FILTER )
    {
    }

    /// <summary>
    /// Constructor accepting a custom filter for weighted
    /// average filtering.
    /// </summary>
    /// <param name="filter">Weighted Average Filter.</param>
    public AverageFilter( int[,] filter )
    {
        this.avgFilter = filter;
        this.filterSum = GetFilterSum(filter);
    }


    private int GetFilterSum( int[,] filter)
    {
        int sum = 0;
        for (int y = 0; y < filter.GetLength(0); y++)
            for (int x = 0; x < filter.GetLength(1); x++)
                sum += filter[x, y];

        return sum;
    }

    /// <summary>
    /// Filters a matrix with the average filter.
    /// </summary>
    /// <param name="initialX">Initial x position.</param>
    /// <param name="initialY">Initial y position.</param>
    /// <param name="matrix">Matrix to be filtered over</param>
    public int Apply(int initialX, int initialY, int[,] matrix)
    {
        int range = avgFilter.GetLength(0) / 2;
        float sum = 0.0f;

        int height = matrix.GetLength(0)-1;
        int width  = matrix.GetLength(1)-1;

        for (int x = - range; x <=  + range; x++)
        {
            for (int y = - range; y <= + range; y++)
            {
                int ix = initialX + x;
                int iy = initialY + y;

                if ( (ix > 0 && ix < width) && ( iy > 0 && iy < height ))
                    sum += matrix[ix,iy] * avgFilter[x+range, y+range];
            }
        }
        return (int)Math.Round(sum / filterSum);
    }
}