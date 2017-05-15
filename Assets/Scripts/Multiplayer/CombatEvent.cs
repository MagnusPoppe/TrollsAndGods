namespace Multiplayer
{
    /// <summary>
    /// A Combat event is a specialized abstract event specified to 
    /// reincarnate a battle.
    /// </summary>
    public abstract class CombatEvent : Event
    {
        private int idFrom, idTo;

        /// <summary>
        /// Initializes a new instance of the <see cref="Multiplayer.CombatEvent"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="description">Description.</param>
        /// <param name="idFrom">Identifier from.</param>
        /// <param name="idTo">Identifier to.</param>
        protected CombatEvent(int id, string description, int idFrom, int idTo) : base(id, description)
        {
            this.idFrom = idFrom;
            this.idTo = idTo;
        }

        /// <summary>
        /// Writes out the event to SQL for easier communitcation with database. 
        /// Only for use with the setup of GameEvent enums.
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