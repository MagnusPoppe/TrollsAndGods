using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class Log
    {
        private List<GameEvent> gameEvents;
        private List<CombatEvent> combatEvents;
        private GameManager gm;

        public Log(GameManager gm)
        {
            gameEvents = new List<GameEvent>();
            combatEvents = new List<CombatEvent>();
            this.gm = gm;
        }

        public void ExectuteAll()
        {
            for (int i = 0; i < gameEvents.Count; i++)
            {
                gameEvents[i].execute();
            }
        }

        public void DownloadEvents()
        {
            // Import only for dummy purposes. TODO: Remove
            gameEvents.Add( new PlayerMoved(gm.Players[0].Heroes[0], new Point(40,16) ) );
            gameEvents.Add( new PlayerMoved(gm.Players[0].Heroes[0], new Point(40,12) ) );
        }

        public void CheckForUpdates()
        {

        }
    }
}
