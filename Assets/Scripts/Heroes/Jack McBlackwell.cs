using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Units;

class JackMcBlackwell : Hero
{
    private const int LOCAL_SPRITE_ID = 0;
    private const int PORTRAIT_ID = 2;
    private const string NAME = "Jack McBlackwell";
    private const string DESCRPITION = "Hi boiz";
    private static UnitTree unitTree;

    // Resources cost: 
    const int GOLD_COST = 1000;
    const int WOOD_COST = 0;
    const int ORE_COST = 0;
    const int CRYSTAL_COST = 0;
    const int GEM_COST = 0;



    public JackMcBlackwell(Player player, Point position) 
        : base(player, position, LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRPITION, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST))
    {
        unitTree = new UnitTree();
        unitTree.addUnit(new StoneTroll(), UnityEngine.Random.Range(2, 4));
        SetUnits(unitTree);
    }

    public JackMcBlackwell()
        : base(LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRPITION, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST))
    {
        unitTree = new UnitTree();
        unitTree.addUnit(new StoneTroll(), UnityEngine.Random.Range(2, 4));
        SetUnits(unitTree);
    }
}
