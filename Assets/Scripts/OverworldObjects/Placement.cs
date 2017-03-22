using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Filter;
using MapGenerator;
using UnityEditor;
using UnityEngine;

namespace OverworldObjects
{
    public class Placement
    {
        private const int NOT_AVAILABLE = 0;
        private const int AVAILABLE = 1;
        private const int MINE_AVAILABLE = 2;

        private int[,] realMap;
        private int[,] canWalk;
        private int[,] placementMap;

        public Placement(int[,] realMap, int[,] canWalk)
        {
            this.realMap = realMap;
            this.canWalk = canWalk;
            placementMap = generatePlacementMap( realMap );
        }

        /// <summary>
        /// Generates a placementmap out of a real map containing all available placements, as
        /// well as placement possibillities for special cases, like mines.
        /// </summary>
        /// <param name="map">Real map</param>
        /// <returns>A 2D array containing all available placements.</returns>
        private int[,] generatePlacementMap(int[,] map)
        {
            int debug_numAvailable = 0;
            int debug_numNotAvailable = 0;
            int debug_numMineAvailable = 0;


            int[,] tempMap = new int[map.GetLength(0), map.GetLength(1)];
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    int tile = map[x, y];

                    if ((IngameObjectLibrary.GetCategory(tile) == IngameObjectLibrary.Category.Ground)
                    && (tile != MapGenerator.MapMaker.WATER_SPRITEID))
                    {
                        if (testForMine(new Point(x,y),  map))
                        {
                            map[x, y] = MINE_AVAILABLE;
                            debug_numMineAvailable++;
                        }
                        else
                        {
                            tempMap[x, y] = AVAILABLE;
                            debug_numAvailable++;
                        }
                    }
                    else
                    {
                        tempMap[x, y] = NOT_AVAILABLE;
                        debug_numNotAvailable++;
                    }
                }
            }
            //Debug.Log("AVAILABLE=" + debug_numAvailable + ",   NOT_AVAILABLE=" + debug_numNotAvailable +",   MINE_AVAILABLE=" + debug_numMineAvailable);
            return tempMap;
        }

        /// <summary>
        /// Finds a placement for a given building inside a region. Uses a rating system
        /// to find the best possible placement for any building.
        ///
        /// NOTE: The best position of a building may be the best position for another
        /// building. Therefore, use the algorithm on the most important buildings or
        /// the ones that have the strictest needs first.
        /// </summary>
        /// <param name="region"> Region to place the building in</param>
        /// <param name="building"> Building to be placed</param>
        /// <returns> coordinate to place the building on, NULL if no available placement. </returns>
        public bool Place(Region region, OverworldBuilding building)
        {
            List<Point> coord = region.GetCoordinates();
            int[,] shape = Shapes.GetShape(building.ShapeType);

            Rating highest = new Rating();
            Point bestPossible = null;

            foreach (Point position in coord)
            {
                Rating other = new Rating();

                // Edge cases:
                if (placementMap[position.x, position.y] != AVAILABLE) continue;
                if (!position.InBounds(placementMap, shape)) continue;

                // Rating the scenery
                other.Scenery( position, shape, realMap );

                // All available buildings for this position:
                bool[] buildable = Shapes.GetBuildingFit(position, placementMap);

                if (buildable[building.ShapeType])
                {
                    other.PossibleBuildings(buildable);
                    if (building.GetType().BaseType == typeof(ResourceBuilding)) // NEEDS TO BE INSIDE A GIVEN RANGE.
                    {
                        ResourceBuilding rs = (ResourceBuilding) building;
                        float actual = position.DistanceTo(region.RegionCenter);

                        if (rs.MinDistFromTown < actual && actual < rs.MaxDistFromTown) // INSIDE RANGE.
                        {
                            other.Distance(rs.MinDistFromTown, actual, rs.MaxDistFromTown);
                        }
                        else // NOT INSIDE RANGE.
                        {
                            continue;
                        }
                    }

                    if (other.Bigger(highest))
                    {
                        bestPossible = position;
                        highest = other;
                    }
                }
            }

            if (bestPossible != null) // PLACING AT BEST POSSIBLE LOCATION:
            {
                // Debug.Log("Best placement: " + bestPossible+",  score="+highest);
                realMap[bestPossible.x, bestPossible.y] = building.GetSpriteID();
                markOccupied(bestPossible, shape);
                return true;
            }

            // NO PLACEMENT FOR THIS BUILDING FOUND...
            return false;
        }

        /// <summary>
        /// Markes the placementmap tiles occupied for
        /// the given placed tiles. JUST AN UPDATE.
        /// </summary>
        /// <param name="shape">Shape of placed object</param>
        private void markOccupied(Point position, int[,] shape)
        {
            for (int iy = 0; iy < shape.GetLength(1); iy++)
            {
                for (int ix = 0; ix < shape.GetLength(0); ix++)
                {
                    int x = position.x + (ix - (shape.GetLength(1)/2));
                    int y = position.y + (iy - (shape.GetLength(0)/2));

                    if (shape[ix, iy] == 1)
                    {
                        placementMap[x, y] = NOT_AVAILABLE;
                    }
                }
            }
        }

        /// <summary>
        /// Tests if there is a possibillity of a mine here.
        /// </summary>
        /// <param name="xy"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        private bool testForMine(Point xy, int[,] map)
        {
            Mountain mountain = new Mountain();

            if (xy.InBounds(map, AverageFilter.DEFAULT_FILTER))
            {
                if (xy.y % 2 == 0) // PAR
                {
                    // POSITBLE RIGHT ORIENTED TRY:
                    Point other = new Point(xy.x+1, xy.y);
                    if (map[other.x, other.y] == mountain.Below) return true;

                    // POSSIBLE LEFT ORIENTED TRY:
                    other = new Point(xy.x - 1, xy.y - 1);
                    if (map[other.x, other.y] == mountain.Below) return true;
                }
                else // ODD
                {

                    // POSITBLE RIGHT ORIENTED TRY:
                    Point other = new Point(xy.x+1, xy.y-1);
                    if (map[other.x, other.y] == mountain.Below) return true;

                    // POSSIBLE LEFT ORIENTED TRY:
                    other = new Point(xy.x, xy.y - 1);
                    if (map[other.x, other.y] == mountain.Below) return true;
                }
            }

            return false;
        }
    }
}