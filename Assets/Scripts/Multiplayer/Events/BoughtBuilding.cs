using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Event for handling a building being bought.
    /// </summary>
    public class BoughtBuilding : GameEvent
    {
        public int buildingID;
        public Point castle;

        public BoughtBuilding(int id, string description, int buildingId, Point castle) : base(id, description)
        {
            buildingID = buildingId;
            this.castle = castle;
        }

        /// <summary>
        /// Executes the action, buying the building.
        /// </summary>
        public override void execute()
        {
            //todo buy building
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Packs this into JSON
        /// </summary>
        /// <returns>JSON of this object</returns>
        public override void unpackJSON(string JSON)
        {
            BoughtBuilding obj = JsonUtility.FromJson<BoughtBuilding>(JSON);
            buildingID = obj.buildingID;
            castle = obj.castle;
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
