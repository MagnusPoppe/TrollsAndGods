
using UnityEngine;

/// <summary>
/// Governs what happens when two heroes meet.
/// </summary>
public class HeroMeetReact : Reaction {

    Hero hero;
    private GameManager gm;

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
        GameObject go = GameObject.Find("GameManager");
        gm = go.GetComponent<GameManager>();
    }

    /// <summary>
    /// If the heroes are friendly to each other, friendly meeting. Else fight.
    /// </summary>
    /// <param name="h">The hero that initiated the meeting</param>
    /// <returns>Returns false</returns>
    public override bool React(Hero h)
    {
        if (hero.Player.equals(h.Player))
        {
            gm.OpenHeroTradePanel(hero, h);
            return false;
        }
        gm.enterCombat(15,11,h,hero);
        Debug.Log("Youve entered combat, this is not yet finished and your stuck now");
        return false;
    }
}
