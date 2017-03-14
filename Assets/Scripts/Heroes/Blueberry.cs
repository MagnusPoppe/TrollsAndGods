using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Blueberry : Hero
{
    private const int LOCAL_SPRITE_ID = 0;
    private const int PORTRAIT_ID = 0;
    private const string NAME = "Blueberry";
    private const string DESCRPITION = "Cool dude, yo bro";

    public Blueberry(Player player, Point position) 
        : base(player, position, LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRPITION)
    {
    }
    public Blueberry()
        : base(LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRPITION)
    {
    }
}
