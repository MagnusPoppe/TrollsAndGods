using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Gork : Hero
{
    private const int LOCAL_SPRITE_ID = 0;
    private const int PORTRAIT_ID = 1;
    private const string NAME = "Gork";
    private const string DESCRPITION = "Yo im'a dorkie dork yo mo";

    public Gork(Player player, Point position) 
        : base(player, position, LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRPITION)
    {
    }

    public Gork()
        : base(LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRPITION)
    {
    }
}
