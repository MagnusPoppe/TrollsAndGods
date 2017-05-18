using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Event for handling units bought from a castle
    /// </summary>
    public class BoughtUnitsFromCastle : GameEvent
    {
        public int heroID, tier, amount;
        public Point pos;

        public BoughtUnitsFromCastle(int id, string description) : base(id, description)
        {
        }

        /// <summary>
        /// Executes the event, buying the units from the castle.
        /// </summary>
        public override void execute()
        {
            //todo buy units
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Packs this into JSON
        /// </summary>
        /// <returns>JSON of this object</returns>
        public override void unpackJSON(string JSON)
        {
            BoughtUnitsFromCastle obj = JsonUtility.FromJson<BoughtUnitsFromCastle>(JSON);
            heroID = obj.heroID;
            tier = obj.tier;
            amount = obj.amount;
            pos = obj.pos;
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
