using System;
using MapGenerator;
using OverworldObjects;
using UnityEngine.PlaymodeTests;

namespace MapGeneratorTests
{
    public class PlacementTester
    {
        [Test()]
        public void TestPlacements()
        {
            int width = 64;
            int height = 64;
            Player[] players = new Player[4];
            String seed = "løkadf";
            int sites = 8;
            int relaxIterations = 3;
            int smoothIterations = 5;
            int fillpercentWalkable = 57;
            int buildingCount = 3;

            MapMaker mapmaker = new MapMaker(
            players, width, height, 40,                     // Map Properites TODO: fjern parameter 40/length
            seed, fillpercentWalkable, smoothIterations,    // BinaryMap Properities
            sites, relaxIterations,                         // Voronoi Properties
            buildingCount,
            false
            );

            Placement _placement = new Placement(mapmaker.GetMap());
        }

    }
}