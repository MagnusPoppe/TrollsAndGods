using System.Linq;
using UnityEngine;

namespace OverworldObjects
{
    public class Rating
    {
        private const bool PREFER_LESS_SPACE = true;
        // ALL FACTORS; RANGING FROM 0 - 10;
        private int distanceFromRegionCenter;
        private int scenery;
        private int possibleBuildings;

        /// <summary>
        /// Default constructor. Sets all rating values to 0.
        /// </summary>
        public Rating()
        {
            distanceFromRegionCenter = 0;
            scenery = 0;
            possibleBuildings = 0;
        }

        /// <summary>
        /// Rates the distance from region center.
        /// Ratings go from 0 - 10.
        /// </summary>
        /// <param name="min"> Minimum distance from region center</param>
        /// <param name="actual">Actual distance from region center</param>
        /// <param name="max"> Maximum distance from region center</param>
        public void Distance(int min, float actual, int max)
        {
            float rating = actual / (max); // GETTING THE SCALED NUMBER;
            distanceFromRegionCenter = (int)(rating * 10.0f);
        }

        /// <summary>
        /// Counts the possible buildings to get a rating. Gives
        /// higher rating to positions where more buildings can be
        /// built.
        /// TODO: Invert the score, so that less buildings is better.
        /// </summary>
        /// <param name="possible">Boolean array from Shapes.GetBuildingFit()</param>
        /// <see cref="Shapes"/>
        public void PossibleBuildings(bool[] possible)
        {
            float count = 0.0f;
            foreach ( bool b in possible) if (b) count += 1;

            float rating = count / (possible.Length +0.0f);

            if (PREFER_LESS_SPACE) possibleBuildings =  10 - (int) (rating * 10.0f) ;
            else possibleBuildings = (int) (rating * 10.0f) ;
        }


        /// <summary>
        /// Gets the total rating for this element.
        /// Ratings go from 0 - 10.
        /// </summary>
        /// <returns>The total rating</returns>
        public int GetRating()
        {
            return distanceFromRegionCenter + scenery + possibleBuildings;
        }

        /// <summary>Tests if the other Rating is higher than this one.</summary>
        /// <param name="other"></param>
        /// <returns>return</returns>
        public bool Bigger(Rating other)
        {
            return (GetRating() > other.GetRating());
        }

        /// <summary>
        /// Uses morphology to rate the tiles around the shape.
        /// TODO: MAKE WORKING VERSION.
        /// NOTES FOR ALGORITHM: Not working because it never finds forest sprites
        /// near the point.
        /// </summary>
        /// <param name="score"></param>
        public void Scenery(Point point, int[,] shape, int[,] realMap)
        {
            int[,] outline = Shapes.GetOutline(shape);
            int total = 0;
            int rating = 0;

            Forest forest = new Forest();
            Grass grass = new Grass();

            for (int y = 0; y < outline.GetLength(0); y++)
            {
                for (int x = 0; x < outline.GetLength(1); x++)
                {
                    if (shape[x, y] == 1) // On the outline:
                    {
                        total++;

                        // Calculating the dx, dy for this point.
                        int dx = point.x + (x - (outline.GetLength(1) / 2));
                        int dy = point.y + (y - (outline.GetLength(1) / 2));

                        foreach (int sprite in forest.Sprites)
                            rating += (sprite == realMap[dx, dy]) ? 1 : 0;
                    }
                }
            }
            if(rating!=0) Debug.Log("RATING="+rating);
            scenery = (int) (rating / (float)total  * 10);
        }

        /// <summary>
        /// Creates a JSON formatted string of the ratings.
        /// </summary>
        /// <returns>String of object.</returns>
        public override string ToString()
        {
            return "{ " +
                       "total:" + GetRating() + ", " +
                       "distance:" + distanceFromRegionCenter + ", " +
                       "scenery:" + scenery + ", " +
                       "PossibleBuildings:"+possibleBuildings +
                   " }";
        }
    }
}