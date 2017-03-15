using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Gork : Hero
{
    private const int LOCAL_SPRITE_ID = 0;
    private const int PORTRAIT_ID = 1;
    private const string NAME = "Gork";
    private const string DESCRIPTION = "Yo im'a dorkie dork yo mo";

    // Resources cost: 
    const int GOLD_COST = 1000;
    const int WOOD_COST = 10;
    const int ORE_COST = 10;
    const int CRYSTAL_COST = 5;
    const int GEM_COST = 0;



    public Gork(Player player, Point position) 
        : base(player, position, LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRIPTION, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST))
    {
    }

    public Gork()
        : base(LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRIPTION, new Cost(GOLD_COST, WOOD_COST, ORE_COST, CRYSTAL_COST, GEM_COST))
    {
    }
}
