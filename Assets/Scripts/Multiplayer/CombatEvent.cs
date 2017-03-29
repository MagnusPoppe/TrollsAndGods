namespace Multiplayer
{
    public abstract class CombatEvent : Event
    {
        private int idFrom, idTo;

        protected CombatEvent(int id, string description, int idFrom, int idTo) : base(id, description)
        {
            this.idFrom = idFrom;
            this.idTo = idTo;
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
    }
}