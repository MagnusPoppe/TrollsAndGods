using System;
namespace Filter
{
	public interface Filter
	{

		int GetFilterSum(int[,] filter);
		int Apply(int initialX, int initialY, int[,] matrix);

	}
}
