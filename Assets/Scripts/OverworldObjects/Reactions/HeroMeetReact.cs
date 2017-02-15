using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMeetReact : Reaction {

    Hero hero;

    public Hero Hero
    {
        get
        {
            return hero;
        }

        set
        {
            hero = value;
        }
    }

    public HeroMeetReact(Hero hero, Vector2 pos)
    {
        Hero = hero;
        Pos = pos;
    }

    public override bool React(Hero h)
    {
        if (hero.Player.Equals(h.Player))
        {
            //TODO friendly meeting
        }
        else
        {
            //TODO fight. if win delete opponent, else delete self. transfer loot and exp.
        }
        
        return true;
    }
}
