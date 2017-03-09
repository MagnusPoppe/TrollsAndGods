﻿
/// <summary>
/// Governs what happens when two heroes meet.
/// </summary>
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

    public HeroMeetReact(Hero hero, Point pos)
    {
        Hero = hero;
        Pos = pos;
    }

    /// <summary>
    /// If the heroes are friendly to each other, friendly meeting. Else fight.
    /// </summary>
    /// <param name="h">The hero that initiated the meeting</param>
    /// <returns>Returns false if friendly meeting, else true</returns>
    public override bool React(Hero h)
    {
        //TODO fight. if win delete opponent, else delete self. transfer loot and exp.
        return true;
    }

    public bool alliedHero(Hero h)
    {
        if (hero.Player.equals(h.Player))
        {
            //TODO friendly meeting
            return true;
        }
        return false;
    }
}
