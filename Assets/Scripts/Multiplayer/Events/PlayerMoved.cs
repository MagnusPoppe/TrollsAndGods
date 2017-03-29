using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Multiplayer
{
    public class PlayerMoved : GameEvent
    {
        private const int ID_FROM = 0;
        private const int ID_TO = 0;
        private const String DESCRIPTION = "Player moves from one (x,y) to another (x,y) in the map.";

        public int playerID;
        public Point from;
        public Point to;

        public PlayerMoved(int playerID, Point from, Point to) : base(ID_FROM, ID_TO, DESCRIPTION)
        {
            this.playerID = playerID;
            this.from = from;
            this.to = to;
        }

        public override void execute()
        {
            throw new System.NotImplementedException();
        }

        public override void unpackJSON( String JSON )
        {
            PlayerMoved  obj = JsonUtility.FromJson<PlayerMoved>(JSON);
            this.playerID = obj.playerID;
            this.from = obj.from;
            this.to = obj.to;
        }

        public override string packJSON()
        {
            return JsonUtility.ToJson(this);
        }
    }
}