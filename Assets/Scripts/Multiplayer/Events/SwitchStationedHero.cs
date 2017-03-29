using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class SwitchStationedHero : GameEvent
    {
        private Point pos;
        private bool stationed;

        public SwitchStationedHero(int id, string description, Point pos, bool stationed) : base(id, description)
        {
            this.pos = pos;
            this.stationed = stationed;
        }

        public override void execute()
        {
            //todo flip stationed hero in castle
            throw new System.NotImplementedException();
        }

        public override void unpackJSON(string JSON)
        {
            SwitchStationedHero obj = JsonUtility.FromJson<SwitchStationedHero>(JSON);
            pos = obj.pos;
            stationed = obj.stationed;
        }

        public override string packJSON()
        {
            return JsonUtility.ToJson(this);
        }
    } 
}
