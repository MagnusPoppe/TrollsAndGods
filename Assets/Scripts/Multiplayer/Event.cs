using System;
using System.Runtime.InteropServices;

namespace Multiplayer
{
    public abstract class Event
    {

        private int id;
        private string description;

        public Event(int id,string description)
        {
            this.id = id;
            this.description = description;
        }

        public abstract void execute();

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Writes out the event to SQL
        /// </summary>
        /// <returns> SQL INSERT SENTENCES. </returns>
        public string toSQLInsert()
        {
            string output = "";
            
            output = "INSERT INTO GameEvent VALUES (" + id + "," + description + ");";
            
            return output;
        }
    }
}
