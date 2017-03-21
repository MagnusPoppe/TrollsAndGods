namespace OverworldObjects
{
    public class Rating
    {
        // ALL FACTORS; RANGING FROM 0 - 10;
        private int distanceFromRegionCenter;
        private int scenery;

        /// <summary>
        /// Rates the distance from region center.
        /// Ratings go from 0 - 10.
        /// </summary>
        /// <param name="min"> Minimum distance from region center</param>
        /// <param name="actual">Actual distance from region center</param>
        /// <param name="max"> Maximum distance from region center</param>
        public void Distance(int min, float actual, int max)
        {
            float rating = actual / (max - min); // GETTING THE SCALED NUMBER;
            distanceFromRegionCenter = (int)(rating * 10);
        }

        public void Scenery(int score)
        {
            scenery = score;
        }

        public int GetRating()
        {
            return distanceFromRegionCenter + scenery;
        }

        public Rating Max(Rating other)
        {
            if (GetRating() < other.GetRating())
                return other;
            return this;
        }
    }
}