using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class JohnyMudbone : Hero
{
    private const int LOCAL_SPRITE_ID = 0;
    private const int PORTRAIT_ID = 3;
    private const string NAME = "Johny Mudbone";
    private const string DESCRPITION = "hi";

    public JohnyMudbone(Player player, Point position) 
        : base(player, position, LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRPITION)
    {
    }
    public JohnyMudbone()
        : base(LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRPITION)
    {
    }
}
