using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class JackMcBlackwell : Hero
{
    private const int LOCAL_SPRITE_ID = 0;
    private const int PORTRAIT_ID = 2;
    private const string NAME = "Jack McBlackwell";
    private const string DESCRPITION = "Hi boiz";

    public JackMcBlackwell(Player player, Point position) 
        : base(player, position, LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRPITION)
    {
    }

    public JackMcBlackwell()
        : base(LOCAL_SPRITE_ID, PORTRAIT_ID, NAME, DESCRPITION)
    {
    }
}
