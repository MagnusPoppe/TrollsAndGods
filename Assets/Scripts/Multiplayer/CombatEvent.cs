using UnityEngine;

namespace Multiplayer
{
    public abstract class CombatEvent : Event
    {
        private int idFrom, idTo;
        private GraphicalBattlefield gb;

        protected CombatEvent(int id, string description, int idFrom, int idTo) : base(id, description)
        {
            this.idFrom = idFrom;
            this.idTo = idTo;
            gb = GameObject.Find("Combat").GetComponent<GraphicalBattlefield>();
        }

        /// <summary>
        /// Writes out the event to SQL
        /// </summary>
        /// <returns> SQL INSERT SENTENCES. </returns>
        public new string toSQLInsert()
        {
            string output = "";
            if (idTo - idFrom > 0)
            {
                for (int i = idFrom; i < idTo; i++)
                    output += "INSERT INTO GameEvent VALUES (" + i + "," + Description + ");";
            }
            else
            {
                output = "INSERT INTO GameEvent VALUES (" + Id + "," + Description + ");";
            }
            return output;
        }

        public int IdFrom
        {
            get { return idFrom; }
            set { idFrom = value; }
        }

        public int IdTo
        {
            get { return idTo; }
            set { idTo = value; }
        }

        public GraphicalBattlefield Gb
        {
            get { return gb; }
            set { gb = value; }
        }
    }
}