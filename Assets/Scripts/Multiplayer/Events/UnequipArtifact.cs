using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Event for a hero unequipping a item
    /// </summary>
    public class UnequipArtifact : GameEvent
    {
        public Point pos;
        public int slot;

        public UnequipArtifact(int id, string description, Point pos, int slot) : base(id, description)
        {
            this.pos = pos;
            this.slot = slot;
        }

        /// <summary>
        /// Executes the action, unequipping the item in the slot from the hero at pos
        /// </summary>
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

        /// <summary>
        /// Unpacks Json into this object
        /// </summary>
        /// <param name="JSON">JSON to be unpacked</param>
        public override void unpackJSON(string JSON)
        {
            UnequipArtifact obj = JsonUtility.FromJson<UnequipArtifact>(JSON);
            pos = obj.pos;
            slot = obj.slot;
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