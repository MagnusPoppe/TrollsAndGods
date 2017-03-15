using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Mantooth : Hero
{
    private const int LOCAL_SPRITE_ID = 0;
    private const int PORTRAIT_ID = 4;
    private const string NAME = "Mantooth";
    private const string DESCRPITION = "Anyone seen ma tooth? It has the shape of a man.";

    public Mantooth(Player player, Point position) 
        : base(player, position, LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRPITION)
    {
    }
    public Mantooth()
        : base(LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRPITION)
    {
    }
}
