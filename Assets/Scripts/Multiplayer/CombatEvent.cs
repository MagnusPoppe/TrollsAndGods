using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// A Combat event is a specialized abstract event specified to 
    /// reincarnate a battle.
    /// </summary>
    public abstract class CombatEvent : Event
    {
        private GraphicalBattlefield gb;


        /// <summary>
        /// Initializes a new instance of the <see cref="Multiplayer.CombatEvent"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="description">Description.</param>
        protected CombatEvent(int id, string description) : base(id, description)
        {
            gb = GameObject.Find("Combat").GetComponent<GraphicalBattlefield>();
        }

        /// <summary>
        /// Writes out the event to SQL for easier communitcation with database. 
        /// Only for use with the setup of GameEvent enums.
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