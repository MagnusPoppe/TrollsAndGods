﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class TestHero : Hero
{
    private const int LOCAL_SPRITE_ID = 0;
    const IngameObjectLibrary.Category SPRITE_CATEGORY = IngameObjectLibrary.Category.ResourceBuildings;

    public TestHero(Player player, Vector2 position) 
        : base(player, position)
    {
        Name = "Testhero";
        CurMovementSpeed = MovementSpeed = 8;
    }
}