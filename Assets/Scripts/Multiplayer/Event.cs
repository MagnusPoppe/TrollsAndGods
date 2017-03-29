using System;
using System.Runtime.InteropServices;

namespace Multiplayer
{
    public abstract class Event
    {
        private int IDfrom, IDto;
        private string description;

        public Event(int IDFrom, int IDTo, string description)
        {
            this.IDfrom = IDFrom;
            this.IDto = IDTo;
            this.description = description;
        }

        public abstract void execute();

        public int Dfrom
        {
            get { return IDfrom; }
            set { IDfrom = value; }
        }

        public int Dto
        {
            get { return IDto; }
            set { IDto = value; }
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
            if (IDto - IDfrom > 0)
            {
                for (int i = IDfrom; i < IDto; i++)
                    output += "INSERT INTO GameEvent VALUES (" + i + "," + description + ");";
            }
            else
            {
                output = "INSERT INTO GameEvent VALUES (" + IDfrom + "," + description + ");";
            }
            return output;
        }
    }
}
