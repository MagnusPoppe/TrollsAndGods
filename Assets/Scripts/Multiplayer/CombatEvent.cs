using UnityEngine;

namespace Multiplayer
{
    public abstract class CombatEvent : Event
    {
        private GraphicalBattlefield gb;

        protected CombatEvent(int id, string description) : base(id, description)
        {
            gb = GameObject.Find("Combat").GetComponent<GraphicalBattlefield>();
        }

        /// <summary>
        /// Writes out the event to SQL
        /// </summary>
        /// <returns> SQL INSERT SENTENCES. </returns>
        public new string toSQLInsert()
        {
            string output = "INSERT INTO GameEvent VALUES (" + Id + "," + Description + ");";
            return output;
        }

        public GraphicalBattlefield Gb
        {
            get { return gb; }
            set { gb = value; }
        }
    }
}