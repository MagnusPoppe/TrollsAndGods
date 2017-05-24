﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Units;

/// <summary>
/// Johny Mudbone hero class
/// </summary>
class JohnyMudbone : Hero
{
    private const int LOCAL_SPRITE_ID = 0;
    private const int PORTRAIT_ID = 3;
    private const string NAME = "Johny Mudbone";
    private const string DESCRIPTION = "hi";
    private static UnitTree unitTree;

    /// <summary>
    /// Resource cost for this hero
    /// </summary>
    const int GOLD_COST = 1000;
    const int WOOD_COST = 0;
    const int ORE_COST = 0;
    const int CRYSTAL_COST = 0;
    const int GEM_COST = 0;


    /// <summary>
    /// Constructor when Hero belongs to a player
    /// </summary>
    /// <param name="player">The player this hero belongs to</param>
    /// <param name="position">Position to spawn the player in</param>
    public JohnyMudbone(Player player, Point position) 
        : base(player, position, LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRIPTION, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST))
    {
        unitTree = new UnitTree();
        unitTree.addUnit(new StoneTroll(), UnityEngine.Random.Range(2, 4));
        SetUnits(unitTree);
    }

    /// <summary>
    /// Default constructor used when the game launches and the Hero doesn't belong to a player
    /// </summary>
    public JohnyMudbone()
        : base(LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRIPTION, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST))
    {
        unitTree = new UnitTree();
        unitTree.addUnit(new StoneTroll(), UnityEngine.Random.Range(2, 4));
        SetUnits(unitTree);
    }
}
