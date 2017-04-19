
namespace MapGenerator
{
	public class TileRating
	{
		float rating;
		Point position;

		public TileRating(float rating, Point position)
		{
			this.rating = rating;
			this.position = position;
		}

		public float Rating
		{
			get
			{
				return rating;
			}

			set
			{
				rating = value;
			}
		}

		public Point Position
		{
			get
			{
				return position;
			}

			set
			{
				position = value;
			}
		}

		//public RateTile(int x, int y, bool[,] map);
	}
}
