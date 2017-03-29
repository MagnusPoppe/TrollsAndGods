using System;
using OverworldObjects;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator
{
	public class Block : IComparable
	{
	    public const int MIN_DISTANCE = 0;
	    public const int MAX_DISTANCE = 1;

	    public Point Position { get; private set; }
	    private int[,] shape;
	    private int shapeType;
	    private Rating rating;

	    public Rating Rate
	    {
	        get { return rating; }
	    }

	    public Block(Point position, int shapeType, int[] distances)
	    {
	        this.Position = position;
	        this.shape = Shapes.GetShape(shapeType);
	    }

	    public bool isSuitable(int[,] canWalk)
	    {
	        return Shapes.GetBuildingFit(Position, canWalk)[shapeType];
	    }

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

	        public int rating()
	        {
	            return distanceFromRegionCenter + scenery;
	        }
	    }

	    public int CompareTo(object obj)
	    {
            Block other = (Block) obj;

	        return (int) (this.Rate.rating() - other.Rate.rating());
	    }
	}
}
