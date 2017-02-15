using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Tavern : Building
    {

        public Tavern()
        {
            name = "Tavern";
            spr = new SpriteRenderer(); // todo
            requirements[0] = true;
            requirements[1] = true;
            requirements[2] = true;
            cost = new Resources(1000, 5, 5, 0, 0);
        }
    }
