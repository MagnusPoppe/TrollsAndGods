namespace MapGenerator
{
    public class BuildingRater
    {
        private Point[] coordinates;
        private Region region;
        private int[,] canWalk;
        private int[,] map;
        private BuildingRating[] ratings;

        public BuildingRater(int[,] map, Region region, int[,] canWalk)
        {
            this.region = region;
            this.coordinates = region.GetCoordinatesArray();

            ratings = new BuildingRating[coordinates.Length];


        }
    }
}