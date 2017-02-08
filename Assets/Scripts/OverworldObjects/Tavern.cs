using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class Tavern
    {
        private bool[,] hexGrid;
        private string name;
        private SpriteRenderer spr;
        public Tavern()
        {
            hexGrid = new bool[2, 2]; // todo
            name = "Tavern";
            spr = new SpriteRenderer(); // todo
        }

        public SpriteRenderer Spr
        {
            get
            {
                return spr;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public bool[,] GetHexGrid()
        {
            return hexGrid;
        }
    }
}

