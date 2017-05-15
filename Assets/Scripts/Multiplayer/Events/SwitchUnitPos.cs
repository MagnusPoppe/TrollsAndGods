using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class SwitchUnitPos : GameEvent
    {
        public Point pos;
        public int from, to;

        public SwitchUnitPos(int id, string description, Point pos, int from, int to) : base(id, description)
        {
            this.pos = pos;
            this.from = from;
            this.to = to;
        }

        public override void execute()
        {
            HeroMeetReact hmr;
            if (Gm.Reactions[pos.x, pos.y].HasPreReact())
            {
                hmr = (HeroMeetReact)Gm.Reactions[pos.x, pos.y].PreReaction;
            }
            else
            {
                hmr = (HeroMeetReact)Gm.Reactions[pos.x, pos.y];
            }
            UnitTree ut = hmr.Hero.Units;
            ut.swapUnits(from, to);
        }

        public override void unpackJSON(string JSON)
        {
            SwitchUnitPos obj = JsonUtility.FromJson<SwitchUnitPos>(JSON);
            pos = obj.pos;
            from = obj.from;
            to = obj.to;
        }

        public override string packJSON()
        {
            return JsonUtility.ToJson(this);
        }
    } 
}
