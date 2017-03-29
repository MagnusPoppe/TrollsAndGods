using System;
using UnityEngine;

namespace Multiplayer
{
    public class PlayerMoved : GameEvent
    {
        private const int ID_FROM = 0;
        private const int ID_TO = 0;
        private const String DESCRIPTION = "Player moves from one (x,y) to another (x,y) in the map.";

        private int playerID;
        private Point from;
        private Point to;

        public PlayerMoved(int id, string description, int playerId, Point from, Point to) : base(id, description)
        {
            playerID = playerId;
            this.from = from;
            this.to = to;
        }

        public override void execute()
        {
            throw new System.NotImplementedException();
        }

        public override string unpackJSON( String JSON )
        {
            
        }

        public override string packJSON()
        {
            return "move:{ " +
                       "player:" + playerID + "," +
                       "from:{x:" + from.x + ",y: " + from.y + "}," +
                       "to:{x:" + to.x + ",y:" + to.y + "}" +
                    "}";
        }
    }
}