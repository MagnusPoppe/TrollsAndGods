using System;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Game events are all events that occur when a player interacts
    /// with the game outside of combat.
    /// </summary>
    public abstract class GameEvent : Event
    {
        protected GameManager gm;

        /// <summary>
        /// Initializes a new instance of the <see cref="Multiplayer.GameEvent"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="description">Description.</param>
        protected GameEvent(int id, string description) : base(id, description)
        {
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        /// <summary>
        /// Unpacks the JSON gotten from the server to interpret the event.
        /// </summary>
        /// <param name="JSON">JSON.</param>
        public abstract void unpackJSON( String JSON );

        /// <summary>
        /// Packs the JSON to send a compressed version of the event to server.
        /// </summary>
        /// <returns>The JSON.</returns>
        public abstract String packJSON();

        public GameManager Gm
        {
            get { return gm; }
            set { gm = value; }
        }
    }
}