using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Switches stationed hero with visiting hero, if there is only one hero it is simply moved.
    /// </summary>
    public class SwitchStationedHero : GameEvent
    {
        public Point pos;
        public bool stationed;

        public SwitchStationedHero(int id, string description, Point pos, bool stationed) : base(id, description)
        {
            this.pos = pos;
            this.stationed = stationed;
        }

        /// <summary>
        /// Executes the action, switching the stationed hero.
        /// </summary>
        public override void execute()
        {
            //todo flip stationed hero in castle
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Unpacks Json into this object
        /// </summary>
        /// <param name="JSON">JSON to be unpacked</param>
        public override void unpackJSON(string JSON)
        {
            SwitchStationedHero obj = JsonUtility.FromJson<SwitchStationedHero>(JSON);
            pos = obj.pos;
            stationed = obj.stationed;
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
