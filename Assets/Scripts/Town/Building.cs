﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using System;

namespace TownView
{


    public class Building : SpriteSystem, Window
    {
        private string name;
        private bool built;
        protected bool[] requirements;
        protected Resources cost;
        private Vector2 placement;
        private const IngameObjectLibrary.Category CATEGORY = IngameObjectLibrary.Category.Town; // TODO: ny kategori for town buildings



        public bool Built
        {
            get
            {
                return built;
            }
        }

        public Vector2 Placement
        {
            get
            {
                return placement;
            }

            set
            {
                placement = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
        
        public void Build()
        {
            // TODO: implementer metoden med cost
            built = true;
        }

        public int UIType()
        {
            throw new NotImplementedException();
        }

        public Building(string name, bool[] requirements, Resources cost, int localID) :base(localID, CATEGORY)
        {
            Name = name;
            this.requirements = requirements;
            Placement = placement;
            this.cost = cost;
        }


    }
}