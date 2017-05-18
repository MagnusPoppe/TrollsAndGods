using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class UnequipArtifact : GameEvent
    {
        public Point pos;
        public int slot;

        public UnequipArtifact(int id, string description, Point pos, int slot) : base(id, description)
        {
            this.pos = pos;
            this.slot = slot;
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
            Item item = hmr.Hero.EquippedItems[slot];
            hmr.Hero.Items.Add(item);
            hmr.Hero.EquippedItems[slot] = null;
        }

        public override void unpackJSON(string JSON)
        {
            UnequipArtifact obj = JsonUtility.FromJson<UnequipArtifact>(JSON);
            pos = obj.pos;
            slot = obj.slot;
        }

        public override string packJSON()
        {
            return JsonUtility.ToJson(this);
        }

    }

}