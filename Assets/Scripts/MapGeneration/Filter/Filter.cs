namespace Filter
{
    /// <summary>
    /// Interface Filter. This was created to have a standard for all
    /// filters to use.
    /// </summary>
	public interface Filter
	{

		int GetFilterSum(int[,] filter);
		int Apply(int initialX, int initialY, int[,] matrix);

	}
}
