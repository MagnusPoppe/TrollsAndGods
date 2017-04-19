using System;
using UnityEngine;

namespace Multiplayer
{
    public abstract class GameEvent : Event
    {
        protected GameManager gm;

        protected GameEvent(int id, string description) : base(id, description)
        {
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        public abstract void unpackJSON( String JSON );
        public abstract String packJSON();

        public GameManager Gm
        {
            get { return gm; }
            set { gm = value; }
        }
    }
}