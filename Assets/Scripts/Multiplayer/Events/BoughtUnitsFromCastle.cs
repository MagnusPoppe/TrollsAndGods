using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class BoughtUnitsFromCastle : GameEvent
    {
        public int heroID, tier, amount;
        public Point pos;

        public BoughtUnitsFromCastle(int id, string description) : base(id, description)
        {
        }

        public override void execute()
        {
            throw new System.NotImplementedException();
        }

        public override void unpackJSON(string JSON)
        {
            BoughtUnitsFromCastle obj = JsonUtility.FromJson<BoughtUnitsFromCastle>(JSON);
            heroID = obj.heroID;
            tier = obj.tier;
            amount = obj.amount;
            pos = obj.pos;
        }

        public override string packJSON()
        {
            return JsonUtility.ToJson(this);
        }
    } 
}
