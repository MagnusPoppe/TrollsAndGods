using OverworldObjects;

namespace MapGenerator
{
    public class BuildingRating
    {
        float[] ratingsPerShape;
        private Point position;

        public BuildingRating(Point position, int[,] map, int[,] canWalk)
        {
            this.position = position;
            ratingsPerShape = new float[Shapes.SHAPE_COUNT];

            bool[] fit = Shapes.GetBuildingFit(position, canWalk);

            for (int i = 0; i < fit.Length; i++)
            {
                if (fit[i]) ratingsPerShape[i] = 0.0f;
                else ratingsPerShape[i] = -1.0f;
            }

            RateAllTypes(map);
        }

        /// <summary>
        /// Rates all buildingtypes for this postio
        /// </summary>
        /// <param name="map"></param>
        public void RateAllTypes(int[,] map)
        {
            for (int i = 0; i < ratingsPerShape.Length; i++)
            {
                if (CanPlaceBuilding(i))
                {
                    int[,] shape = Shapes.GetShape(i);
                    for (int y = 0; y < shape.GetLength(1); y++)
                    {
                        for (int x = 0; x < shape.GetLength(0); x++)
                        {
                        }
                    }
                }
            }
        }

        public bool CanPlaceBuilding(int shapetype)
        {
            return ratingsPerShape[shapetype] >= 0.0f;
        }
    }
}