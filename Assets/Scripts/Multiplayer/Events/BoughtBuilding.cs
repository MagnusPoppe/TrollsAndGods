using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class BoughtBuilding : GameEvent
    {
        public int buildingID;
        public Point castle;

        public BoughtBuilding(int id, string description, int buildingId, Point castle) : base(id, description)
        {
            buildingID = buildingId;
            this.castle = castle;
        }

        public override void execute()
        {
            //todo buy building
            throw new System.NotImplementedException();
        }

        public override void unpackJSON(string JSON)
        {
            BoughtBuilding obj = JsonUtility.FromJson<BoughtBuilding>(JSON);
            buildingID = obj.buildingID;
            castle = obj.castle;
        }

        public override string packJSON()
        {
            return JsonUtility.ToJson(this);
        }
    } 
}
