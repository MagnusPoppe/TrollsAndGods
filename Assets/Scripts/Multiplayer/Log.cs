using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// The Log is a set of events that has happened in a game. We create
    /// separate lists for combat events and game events. When saving a game
    /// in the database for many players, we would want to save only the results
    /// of a battle, and not the whole thing. Combat events will therefore be more 
    /// of a cache, and cannot be stored with other events.
    /// </summary>
    public class Log
    {
        private List<GameEvent> gameEvents;
        private List<CombatEvent> combatEvents;
        private GameManager gm;

        /// <summary>
        /// Initializes a new instance of the <see cref="Multiplayer.Log"/> class.
        /// </summary>
        /// <param name="gm">Gm.</param>
        public Log(GameManager gm)
        {
            gameEvents = new List<GameEvent>();
            combatEvents = new List<CombatEvent>();
            this.gm = gm;
        }

        /// <summary>
        /// Exectutes all events in the order the have been created.
        /// </summary>
        public void ExectuteAll()
        {
            for (int i = 0; i < gameEvents.Count; i++)
            {
                gameEvents[i].execute();
            }
        }

        /// <summary>
        /// Downloads the events from server.
        /// TODO: Remove hardcoded data.
        /// </summary>
        public void DownloadEvents()
        {
            // Import only for dummy purposes. TODO: Remove
            gameEvents.Add( new PlayerMoved(gm.Players[0].Heroes[0], new Point(40,16) ) );
            gameEvents.Add( new PlayerMoved(gm.Players[0].Heroes[0], new Point(40,12) ) );
        }

        /// <summary>
        /// Checks for updates on the server for this given game.
        /// </summary>
        public void CheckForUpdates()
        {
            // TODO: NOT YET IMPLEMENTED.
        }
    }
}
