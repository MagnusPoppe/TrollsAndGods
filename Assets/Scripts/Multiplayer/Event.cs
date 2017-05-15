using System;
using System.Runtime.InteropServices;

namespace Multiplayer
{
    /// <summary>
    /// Event. Something that happens within the game. Every interaction
    /// a player can have with the game is an event. The eventsystem is then
    /// created to make all player based events into commands so that the game
    /// can be re-run with just code. 
    /// </summary>
    public abstract class Event
    {

        private int id;
        private string description;

        /// <summary>
        /// Initializes a new instance of the <see cref="Multiplayer.Event"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="description">Description.</param>
        public Event(int id,string description)
        {
            this.id = id;
            this.description = description;
        }

        /// <summary>
        /// Execute the event.
        /// </summary>
        public abstract void execute();

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
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
