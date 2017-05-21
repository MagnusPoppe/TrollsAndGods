using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Event for a hero equipping a Artifact.
    /// </summary>
    public class EquipArtifact : GameEvent
    {
        public Point pos;
        public int slot;

        public EquipArtifact(int id, string description, Point pos, int slot) : base(id, description)
        {
            this.pos = pos;
            this.slot = slot;
        }

        /// <summary>
        /// Executes the action, equipping the Artifact, moving anything already equipped in slot to inventory.
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
            Item item = hmr.Hero.Items[slot];
            Item tmp = hmr.Hero.EquippedItems[item.SlotType];
            hmr.Hero.EquippedItems[item.SlotType] = item;
            hmr.Hero.Items[slot] = tmp;
        }

        /// <summary>
        /// Unpacks Json into this object
        /// </summary>
        /// <param name="JSON">JSON to be unpacked</param>
        public override void unpackJSON(string JSON)
        {
            EquipArtifact obj = JsonUtility.FromJson<EquipArtifact>(JSON);
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
