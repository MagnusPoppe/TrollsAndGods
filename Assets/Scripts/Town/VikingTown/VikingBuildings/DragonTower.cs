using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TownView
{
    /// <summary>
    /// Placeholder class for a spesific building.
    /// Belongs to the "Unknown Town".
    /// </summary>
    public class DragonTower : Building
    {
        // Required values for building:
        const string name = "Dragon Tower";
        const bool[] requirements = null;
        const int LOCAL_SPRITEID = 4;


        // Resources cost: 
        const int GOLD_COST = 0;
        const int WOOD_COST = 0;
        const int ORE_COST = 0;
        const int CRYSTAL_COST = 0;
        const int GEM_COST = 0;



        // This needs no indata since it knows its values.
        public DragonTower() : base(name, requirements, new Resources(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST), LOCAL_SPRITEID)
        {
        }
    }
}
