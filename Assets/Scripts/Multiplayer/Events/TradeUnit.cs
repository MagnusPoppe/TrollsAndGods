using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Event for heroes trading a unit between eachother
    /// </summary>
    public class TradeUnit : GameEvent
    {
        public Point fromHero, toHero;
        public int fromSlot, toSlot;

        public TradeUnit(int id, string description, Point fromHero, Point toHero, int fromSlot, int toSlot) : base(id, description)
        {
            this.fromHero = fromHero;
            this.toHero = toHero;
            this.fromSlot = fromSlot;
            this.toSlot = toSlot;
        }

        /// <summary>
        /// Executes the action, swapping unit in fromSlot with unit in toSlot.
        /// </summary>
        public override void execute()
        {
            HeroMeetReact hmr1;
            if (Gm.Reactions[fromHero.x, fromHero.y].HasPreReact())
            {
                hmr1 = (HeroMeetReact)Gm.Reactions[fromHero.x, fromHero.y].PreReaction;
            }
            else
            {
                hmr1 = (HeroMeetReact)Gm.Reactions[fromHero.x, fromHero.y];
            }
            HeroMeetReact hmr2;
            if (Gm.Reactions[toHero.x, toHero.y].HasPreReact())
            {
                hmr2 = (HeroMeetReact)Gm.Reactions[toHero.x, toHero.y].PreReaction;
            }
            else
            {
                hmr2 = (HeroMeetReact)Gm.Reactions[toHero.x, toHero.y];
            }
            UnitTree utFrom = hmr1.Hero.Units;
            UnitTree utTo = hmr2.Hero.Units;

            Unit tmp = utTo.GetUnits()[toSlot];
            int tmpAmount = utTo.getUnitAmount(toSlot);

            utTo.setUnit(utFrom.GetUnits()[fromSlot],utFrom.getUnitAmount(fromSlot),toSlot);
            utFrom.setUnit(tmp,tmpAmount,fromSlot);
        }

        /// <summary>
        /// Unpacks Json into this object
        /// </summary>
        /// <param name="JSON">JSON to be unpacked</param>
        public override void unpackJSON(string JSON)
        {
            TradeUnit obj = JsonUtility.FromJson<TradeUnit>(JSON);
            fromHero = obj.fromHero;
            toHero = obj.toHero;
            fromSlot = obj.fromSlot;
            toSlot = obj.toSlot;
        }

        /// <summary>
        /// Packs this into JSON
        /// </summary>
        /// <returns>JSON of this object</returns>
        public override string packJSON()
        {
            return JsonUtility.ToJson(this);
        }
    } 
}
