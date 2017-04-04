using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
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

        public override void execute()
        {
            //todo implement
            throw new System.NotImplementedException();
        }

        public override void unpackJSON(string JSON)
        {
            TradeUnit obj = JsonUtility.FromJson<TradeUnit>(JSON);
            fromHero = obj.fromHero;
            toHero = obj.toHero;
            fromSlot = obj.fromSlot;
            toSlot = obj.toSlot;
        }

        public override string packJSON()
        {
            return JsonUtility.ToJson(this);
        }
    } 
}
