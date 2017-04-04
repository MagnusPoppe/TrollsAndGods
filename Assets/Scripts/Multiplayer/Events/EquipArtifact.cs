using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class EquipArtifact : GameEvent
    {
        public Point pos;
        public int slot;

        public EquipArtifact(int id, string description, Point pos, int slot) : base(id, description)
        {
            this.pos = pos;
            this.slot = slot;
        }

        public override void execute()
        {
            //todo implement
            throw new System.NotImplementedException();
        }

        public override void unpackJSON(string JSON)
        {
            EquipArtifact obj = JsonUtility.FromJson<EquipArtifact>(JSON);
            pos = obj.pos;
            slot = obj.slot;
        }

        public override string packJSON()
        {
            return JsonUtility.ToJson(this);
        }
    } 
}
