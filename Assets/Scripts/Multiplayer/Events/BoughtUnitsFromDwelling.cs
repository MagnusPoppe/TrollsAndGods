using System.Collections;
using System.Collections.Generic;
using OverworldObjects;
using UnityEngine;

namespace Multiplayer
{
    public class BoughtUnitsFromDwelling : GameEvent
    {
        public int amount, heroID;
        public Point dwelling;


        public BoughtUnitsFromDwelling(int id, string description, int amount, int heroId, Point dwelling) : base(id, description)
        {
            this.amount = amount;
            heroID = heroId;
            this.dwelling = dwelling;
        }

        public override void execute()
        {
            //todo buy units
            throw new System.NotImplementedException();
        }

        public override void unpackJSON(string JSON)
        {
            BoughtUnitsFromDwelling obj = JsonUtility.FromJson<BoughtUnitsFromDwelling>(JSON);
            amount = obj.amount;
            heroID = obj.heroID;
            dwelling = obj.dwelling;
        }

        public override string packJSON()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
